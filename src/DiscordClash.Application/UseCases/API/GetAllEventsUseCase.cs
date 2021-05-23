using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordClash.Application.UseCases.API
{
    public class GetAllEventsUseCase
    {
        private readonly IGenericRepository<Choice> _eventRepository;
        private readonly ILogger<GetAllEventsUseCase> _logger;

        public GetAllEventsUseCase(IGenericRepository<Choice> eventRepository, ILogger<GetAllEventsUseCase> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Choice>> Execute()
        {
            var events = await _eventRepository.GetAll();
            _logger.LogInformation("Fetched all events");
            return events;
        }
    }
}
