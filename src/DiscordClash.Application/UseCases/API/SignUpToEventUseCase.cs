using AutoMapper;
using DiscordClash.Application.Commands;
using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordClash.Application.UseCases.API
{
    public class SignUpToEventUseCase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Choice> _choiceRepository;
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly ILogger<CreateNewEventUseCase> _logger;

        public SignUpToEventUseCase(IMapper mapper, IGenericRepository<User> userRepository, ILogger<CreateNewEventUseCase> logger, IGenericRepository<Choice> choiceRepository, IGenericRepository<Event> eventRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
            _choiceRepository = choiceRepository;
            _eventRepository = eventRepository;
        }

        public async Task Execute(SignUpToEvent cmd)
        {
            //todo: check if user exists
            var @event = await _eventRepository.Get(cmd.EventId);
            if (@event is null) throw new KeyNotFoundException($"Event with id: {cmd.EventId} does not exists.");

            var user = _mapper.Map<User>(cmd.User);
            await _userRepository.Add(user);
            _logger.LogInformation("New user was added to DB: {@user}", user);

            //todo: UserId missing
            var choice = _mapper.Map<Choice>(cmd); //, opt => opt.BeforeMap((src, dest) => dest.UserId = user.Id));
            await _choiceRepository.Add(choice);
            _logger.LogInformation("New user's choice was added to DB: {@choice}", choice);

            //todo: when all
        }
    }
}
