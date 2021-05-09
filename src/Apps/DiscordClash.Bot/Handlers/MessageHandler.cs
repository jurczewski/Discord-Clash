using System.Threading.Tasks;
using DiscordClash.Application.Messages;
using DiscordClash.Application.UseCases;
using EasyNetQ;
using Microsoft.Extensions.Logging;

namespace DiscordClash.Bot.Handlers
{
    public class MessageHandler
    {
        private readonly IBus _bus;
        private readonly NotifyAboutNewEventUseCase _newEventUseCase; //todo: make sure it is okay
        private readonly ILogger<MessageHandler> _logger;

        public MessageHandler(IBus bus, NotifyAboutNewEventUseCase newEventUseCase, ILogger<MessageHandler> logger)
        {
            _bus = bus;
            _newEventUseCase = newEventUseCase;
            _logger = logger;
        }

        public void ProcessMessages()
        {
            _bus.SendReceive.Receive<NewEvent>(Queues.Events, HandleMessage);
        }

        private async Task HandleMessage(NewEvent msg)
        {
            _logger.LogInformation("Received message with new event. {msg}", msg);
            await _newEventUseCase.Execute(msg);
        }
    }
}
