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
        private readonly NotifyAboutNewEventUseCase _notifyAboutNewEventUseCase;
        private readonly ILogger<MessageHandler> _logger;

        public MessageHandler(IBus bus, NotifyAboutNewEventUseCase notifyAboutNewEventUseCase, ILogger<MessageHandler> logger)
        {
            _bus = bus;
            _notifyAboutNewEventUseCase = notifyAboutNewEventUseCase;
            _logger = logger;
        }

        public void ProcessMessages()
        {
            _bus.SendReceive.Receive<NewEvent>(Queues.NewEvent, HandleMessage);
        }

        private async Task HandleMessage(NewEvent msg)
        {
            _logger.LogInformation("Received message with new event. {msg}", msg);
            await _notifyAboutNewEventUseCase.Execute(msg);
        }
    }
}
