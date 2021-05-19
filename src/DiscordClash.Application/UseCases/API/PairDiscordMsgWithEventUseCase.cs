using System.Threading.Tasks;
using DiscordClash.Application.Messages;
using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace DiscordClash.Application.UseCases.API
{
    public class PairDiscordMsgWithEventUseCase //todo: consider an api call instead of rabbit
    {
        private readonly ILogger<PairDiscordMsgWithEventUseCase> _logger;
        private readonly IGenericRepository<Event> _eventRepository;

        public PairDiscordMsgWithEventUseCase(ILogger<PairDiscordMsgWithEventUseCase> logger, IGenericRepository<Event> eventRepository)
        {
            _logger = logger;
            _eventRepository = eventRepository;
        }
        public async Task Execute(PairDiscordMsgWithEvent discordMsg)
        {
            var @event = await _eventRepository.Get(discordMsg.EventId);
            @event.SetDiscordMessageId(discordMsg.DiscordMsgId);
            await _eventRepository.Update(@event);
            _logger.LogInformation("Event with id: {@id} was paired with Discord message: {@msgId}", @event.Id, @event.DiscordMessageId);
        }
    }
}
