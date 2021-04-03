using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InventoryTracker.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        #region Members
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        #endregion


        #region Constructor
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        #endregion


        #region Public Methods
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CustomException cex)
            {
                object errors = cex.Errors;
                await HandleException(cex, httpContext, errors, (int)cex.StatusCode);
            }
            catch (ValidationException vex)
            {
                object errors = vex.Message;
                await HandleException(vex, httpContext, errors, (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                object errors = !string.IsNullOrWhiteSpace(ex.Message) ? ex.Message : "Oops, something went wrong";
                await HandleException(ex, httpContext, errors, (int)HttpStatusCode.InternalServerError);
            }
        }
        #endregion


        #region Private Methods
        private async Task HandleException(Exception ex, HttpContext httpContext, object errors, int statusCode)
        {
            //Note: Please be careful when logging request body, it may contain sensitive information.
            _logger.LogError(ex, $"RequestBody:{await GetBodyAsync(httpContext.Request)}");
            httpContext.Response.StatusCode = statusCode;
            if (errors != null)
            {
                string errorBody = JsonSerializer.Serialize(new { errors });
                httpContext.Response.ContentType = "application/problem+json";
                var problem = new ProblemDetails
                {
                    Status = statusCode,
                    Detail = errorBody
                };

                //Serialize the problem details object to the Response as JSON (using System.Text.Json)
                var stream = httpContext.Response.Body;
                await JsonSerializer.SerializeAsync(stream, problem);
            }
        }

        public static async Task<string> GetBodyAsync(HttpRequest request)
        {
            if (request.ContentLength > 0)
            {
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                string bodyAsText = Encoding.UTF8.GetString(buffer);
                request.Body.Seek(0, SeekOrigin.Begin);
                return bodyAsText;
            }
            return default;
        }
        #endregion
    }
}