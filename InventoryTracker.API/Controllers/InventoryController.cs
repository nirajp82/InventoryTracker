using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InventoryTracker.Application;
using InventoryTracker.Dto;
using InventoryTracker.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.API.Controllers
{
    [ApiController]
    public class InventoryController : BaseController
    {
        #region Get Action Methods
        [HttpGet("{name:minlength(1):maxlength(50)}")]
        [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ServiceFilter(typeof(ValidateItemExistsFilter))]
        public async Task<IActionResult> Get(string name, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new Get.Query(name), cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new List.Query(), cancellationToken);
            if (result != null)
                return Ok(result);
            else
                return NoContent();
        }
        #endregion


        #region Command Action Methods
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] IEnumerable<SaveList.Command.Item> list, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(new SaveList.Command { List = list }, cancellationToken);
            return Ok(response);
        }


        [HttpPut("{name:minlength(1):maxlength(50)}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(string name, [FromBody] Save.Command request,
            CancellationToken cancellationToken)
        {
            request.Name = name;
            await Mediator.Send(request, cancellationToken);
            if (!request.Version.HasValue)
                return CreatedAtAction(nameof(Get), name);
            else
                return NoContent();
        }


        [HttpDelete("{name:minlength(1):maxlength(50)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ServiceFilter(typeof(ValidateItemExistsFilter))]
        public async Task<IActionResult> Delete(string name, CancellationToken cancellationToken)
        {
            await Mediator.Send(new Delete.Command { Name = name }, cancellationToken);
            return NoContent();
        }
        #endregion
    }
}
