using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        #region Private Members
        private IMediator _mediator { get; set; }
        #endregion


        #region Public Members
        protected IMediator Mediator => _mediator ??
                    (_mediator = HttpContext.RequestServices.GetService<IMediator>());
        #endregion
    }
}