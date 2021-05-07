using DiscordClash.Application.Messages;
using DiscordClash.Application.UseCases;
using EasyNetQ;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DiscordClash.Bot.Services
{
    public class MessageService
    {
        private readonly IBus _bus;
        private readonly NotifyAboutNewEventUseCase _newEventUseCase; //todo: make sure it is okay
        private readonly ILogger<MessageService> _logger;

        public MessageService(IBus bus, NotifyAboutNewEventUseCase newEventUseCase, ILogger<MessageService> logger)
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
