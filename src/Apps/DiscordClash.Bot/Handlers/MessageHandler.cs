using AutoMapper;
using DiscordClash.Application.Messages;
using DiscordClash.Application.Requests;
using EasyNetQ;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DiscordClash.Bot.Handlers
{
    public class MessageHandler
    {
        private readonly IBus _bus;
        private readonly ILogger<MessageHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MessageHandler(IBus bus, ILogger<MessageHandler> logger, IMediator mediator, IMapper mapper)
        {
            _bus = bus;
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        public void ProcessMessages()
        {
            _bus.SendReceive.Receive<NewEvent>(Queues.Events, HandleMessage);
        }

        private async Task HandleMessage(NewEvent msg)
        {
            _logger.LogInformation("Received message with new event. {msg}", msg);
            var request = _mapper.Map<NotifyAboutEvent>(msg);
            await _mediator.Send(request);
        }
    }
}
