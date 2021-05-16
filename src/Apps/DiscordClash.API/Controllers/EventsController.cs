using DiscordClash.Application.Commands;
using DiscordClash.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DiscordClash.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        /// <summary>
        /// Create new event.
        /// </summary>
        /// <param name="apiUseCase"></param>
        /// <param name="cmd"></param>
        /// <response code="201">Event was created. Its Id is returned in Location.</response>
        /// <response code="400">Event already exists.</response>
        [HttpPost]
        public async Task<IActionResult> CreateNewEvent([FromServices] CreateNewEventUseCase apiUseCase, CreateNewEvent cmd)
        {
            var guid = Guid.NewGuid();
            cmd.Id = guid;
            await apiUseCase.Execute(cmd);

            return new CreatedResult($"{guid}", null);
        }

        /// <summary>
        /// Remove event.
        /// </summary>
        /// <param name="apiUseCase"></param>
        /// <param name="id">Event id.</param>
        /// <response code="204">Event was successfully updated.</response>
        [HttpDelete]
        public async Task<IActionResult> RemoveEvent([FromServices] RemoveEventUseCase apiUseCase, Guid id)
        {
            await apiUseCase.Execute(id);
            return NoContent();
        }
    }
}
