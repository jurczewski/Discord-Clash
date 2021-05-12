using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using DiscordClash.Application.Messages;
using DiscordClash.Application.Requests;
using DiscordClash.Core.Domain;
using Game = DiscordClash.Application.Requests.Game;

namespace DiscordClash.Application.Mappings
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<CreateNewEvent, NewEvent>();
            CreateMap<CreateNewEvent, Event>();
            CreateMap<NewEvent, NotifyAboutEvent>();

            CreateMap<Game, Messages.Game>().ConvertUsingEnumMapping();
            CreateMap<Game, Core.Domain.Game>().ConvertUsingEnumMapping();
        }
    }
}
