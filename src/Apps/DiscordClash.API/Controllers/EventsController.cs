using DiscordClash.Application.Commands;
using DiscordClash.Application.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DiscordClash.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IBus _bus;

        public EventsController(IBus bus)
        {
            _bus = bus;
        }

        /// <summary>
        /// Create new event.
        /// </summary>
        /// <param name="cmd"></param>
        /// <response code="201">Event was created. Its Id is returned in Location.</response>
        /// <response code="400">Event already exists.</response>
        [HttpPost]
        public async Task<IActionResult> CreateNewEvent(CreateNewEvent cmd)
        {
            // todo: add proper services
            var guid = Guid.NewGuid();

            // pass details to events service - create new event and add to db
            //await _orderService.CreateOrder(guid, customerId, cmd);

            // use rabbit and send it to bot (notification service?)

            // add dto for api, messages

            // return a result code

            await _bus.SendReceive.SendAsync(Queues.Events, cmd);

            return new CreatedResult($"{guid}", null);
        }
    }
}
