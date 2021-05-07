using DiscordClash.Application.Commands;
using DiscordClash.Application.Messages;
using EasyNetQ;
using System.Threading.Tasks;
using AutoMapper;
using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;

namespace DiscordClash.Application.UseCases
{
    public class CreateNewEventUseCase
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;

        public CreateNewEventUseCase(IBus bus, IMapper mapper, IEventRepository eventRepository)
        {
            _bus = bus;
            _mapper = mapper;
            _eventRepository = eventRepository;
        }

        public async Task Execute(CreateNewEvent cmd)
        {
            var msg = _mapper.Map<NewEvent>(cmd);
            await _bus.SendReceive.SendAsync(Queues.Events, msg);

            var @event = _mapper.Map<Event>(cmd);
            await _eventRepository.Add(@event);
        }
    }
}
