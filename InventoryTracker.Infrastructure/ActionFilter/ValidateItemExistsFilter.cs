using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using static InventoryTracker.Application.Exists;

namespace InventoryTracker.Infrastructure
{
    public class ValidateItemExistsFilter : IAsyncActionFilter
    {
        #region Member
        private IMediator _mediator { get; }
        private ILogger<ValidateItemExistsFilter> _logger { get; }
        #endregion


        #region Constructor
        public ValidateItemExistsFilter(IMediator mediator, ILogger<ValidateItemExistsFilter> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        #endregion


        #region Method
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments["name"] is string name)
            {
                //Check if record exists, Send Request Exists
                if (await _mediator.Send(new Query(name)))
                {
                    await next();
                    return;
                }
                else
                    throw new CustomException(HttpStatusCode.NotFound, new { Param = $"Record not found for {name}" });
            }
            throw new ValidationException("Missing name parameter!");
        }
        #endregion
    }
}