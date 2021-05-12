using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using DiscordClash.Application.Requests;

namespace DiscordClash.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create new event.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Event was created. Its Id is returned in Location.</response>
        /// <response code="400">Event already exists.</response>
        [HttpPost]
        public async Task<IActionResult> CreateNewEvent(CreateNewEvent cmd, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(cmd, cancellationToken);

            return new CreatedResult($"{id}", null);
        }
    }
}
