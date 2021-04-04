using InventoryTracker.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace InventoryTracker.Infrastructure
{
    public class ValidateItemSearch : IAsyncActionFilter
    {
        #region Member
        private readonly IMediator _mediator;
        #endregion


        #region Constructor
        public ValidateItemSearch(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion


        #region Method
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments[nameof(Search.Query)] is Search.Query searchQuery)
            {
                //Check if record exists, Send Request Exists
                if (await _mediator.Send(new ValidateSearch.Query { Offset = searchQuery.Offset, Limit = searchQuery.Limit, NameLike = searchQuery.NameLike }))
                {
                    await next();
                    return;
                }
                else
                    throw new CustomException(HttpStatusCode.BadRequest, new { Param = $"Invalid Search Criteria" });
            }
        }
        #endregion
    }
}