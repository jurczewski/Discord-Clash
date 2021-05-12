using AutoMapper;
using DiscordClash.Application.Messages;
using DiscordClash.Application.Requests;
using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using EasyNetQ;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordClash.Application.Handlers
{
    public class CreateNewEventHandler : IRequestHandler<CreateNewEvent, Guid>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<CreateNewEventHandler> _logger;

        public CreateNewEventHandler(IBus bus, IMapper mapper, IEventRepository eventRepository, ILogger<CreateNewEventHandler> logger)
        {
            _bus = bus;
            _mapper = mapper;
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateNewEvent cmd, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            cmd.Id = id;

            var msg = _mapper.Map<NewEvent>(cmd);
            await _bus.SendReceive.SendAsync(Queues.Events, msg, cancellationToken);
            _logger.LogInformation("CreateNewEvent message was send - {@msg}", msg);

            var @event = _mapper.Map<Event>(cmd);
            await _eventRepository.Add(@event);
            _logger.LogInformation("New event was added - {@event}", @event);

            return id;
        }
    }
}
