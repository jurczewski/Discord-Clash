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
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Choice> _choiceRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<CreateNewEventUseCase> _logger;

        public SignUpToEventUseCase(IMapper mapper, IUserRepository userRepository, ILogger<CreateNewEventUseCase> logger, IGenericRepository<Choice> choiceRepository, IEventRepository eventRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
            _choiceRepository = choiceRepository;
            _eventRepository = eventRepository;
        }

        public async Task Execute(SignUpToEvent cmd)
        {
            var user = await _userRepository.GetByDiscordId(cmd.User.DiscordId);
            if (user is null)
            {
                user = _mapper.Map<User>(cmd.User);
                await _userRepository.Add(user);
                _logger.LogInformation("New user was added: {@user}", user);
            }

            var @event = await _eventRepository.GetByMessageId(cmd.EventMsgId);
            if (@event is null) throw new KeyNotFoundException($"Event with discord msg id: {cmd.EventMsgId} does not exists.");

            var choice = _mapper.Map<Choice>(cmd, opt => opt.AfterMap((_, dest) =>
            {
                dest.SetUserId(user.Id);
                dest.SetEventId(@event.Id);
            }));

            await _choiceRepository.Add(choice);
            _logger.LogInformation("New user's choice was added to DB: {@choice}", choice);
        }
    }
}
