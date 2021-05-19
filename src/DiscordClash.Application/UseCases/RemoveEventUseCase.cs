using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordClash.Application.UseCases
{
    public class RemoveEventUseCase
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly ILogger<CreateNewEventUseCase> _logger;

        public RemoveEventUseCase(IGenericRepository<Event> eventRepository, ILogger<CreateNewEventUseCase> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public async Task Execute(Guid id)
        {
            var @event = await _eventRepository.Get(id);
            if (@event is null)
            {
                throw new KeyNotFoundException($"Event with id: '{id}', does not exists.");
            }
            await _eventRepository.Delete(id);
            _logger.LogInformation("Event was removed - {@event}", @event);
        }
    }
}
