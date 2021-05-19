using AutoMapper;
using DiscordClash.Application.Commands;
using DiscordClash.Application.Messages;
using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using EasyNetQ;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DiscordClash.Application.UseCases
{
    public class CreateNewEventUseCase
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly ILogger<CreateNewEventUseCase> _logger;

        public CreateNewEventUseCase(IBus bus, IMapper mapper, IGenericRepository<Event> eventRepository, ILogger<CreateNewEventUseCase> logger)
        {
            _bus = bus;
            _mapper = mapper;
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public async Task Execute(CreateNewEvent cmd)
        {
            var msg = _mapper.Map<NewEvent>(cmd);
            await _bus.SendReceive.SendAsync(Queues.NewEvent, msg);
            _logger.LogInformation("CreateNewEvent message was send - {@msg}", msg);

            var @event = _mapper.Map<Event>(cmd);
            await _eventRepository.Add(@event);
            _logger.LogInformation("New event was added - {@event}", @event);
        }
    }
}
