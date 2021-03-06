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
        private readonly IMediator _mediator;
        #endregion


        #region Constructor
        public ValidateItemExistsFilter(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion


        #region Method
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments[nameof(Query.Name)] is string name)
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