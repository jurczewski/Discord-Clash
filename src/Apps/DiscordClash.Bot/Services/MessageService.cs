using DiscordClash.Application.Commands;
using DiscordClash.Application.Messages;
using DiscordClash.Application.Services.Interfaces;
using EasyNetQ;
using Microsoft.Extensions.Logging;

namespace DiscordClash.Bot.Services
{
    public class MessageService
    {
        private readonly IBus _bus;
        private readonly INotificationService _notificationService;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IBus bus, INotificationService notificationService, ILogger<MessageService> logger)
        {
            _bus = bus;
            _notificationService = notificationService;
            _logger = logger;
        }

        public void ProcessMessages()
        {
            _bus.SendReceive.Receive<CreateNewEvent>(Queues.Events, HandleMessage);
        }

        private void HandleMessage(CreateNewEvent msg)
        {
            _logger.LogInformation("Received message with new event. {msg}", msg);
            _notificationService.NotifyAboutNewEvent(msg);
        }
    }
}
