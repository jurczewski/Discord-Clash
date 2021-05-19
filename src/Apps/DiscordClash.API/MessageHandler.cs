using DiscordClash.Application.Messages;
using EasyNetQ;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DiscordClash.Application.UseCases.API;

namespace DiscordClash.API
{
    public class MessageHandler
    {
        private readonly IBus _bus;
        private readonly PairDiscordMsgWithEventUseCase _pairDiscordMsgWithEvent;
        private readonly ILogger<MessageHandler> _logger;

        public MessageHandler(IBus bus, ILogger<MessageHandler> logger, PairDiscordMsgWithEventUseCase pairDiscordMsgWithEvent)
        {
            _bus = bus;
            _logger = logger;
            _pairDiscordMsgWithEvent = pairDiscordMsgWithEvent;
        }

        public void ProcessMessages()
        {
            _bus.SendReceive.Receive<PairDiscordMsgWithEvent>(Queues.PairMsgWithEvent, HandleMessage);
        }

        private async Task HandleMessage(PairDiscordMsgWithEvent msg)
        {
            _logger.LogInformation("Received new message: {msg}", msg);
            await _pairDiscordMsgWithEvent.Execute(msg);
        }
    }
}
