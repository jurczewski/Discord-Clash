using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using DiscordClash.Application.Commands;
using DiscordClash.Application.Messages;
using DiscordClash.Core.Domain;

namespace DiscordClash.Application.Mappings
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<CreateNewEvent, NewEvent>();
            CreateMap<CreateNewEvent, Event>();

            CreateMap<Commands.Game, Messages.Game>().ConvertUsingEnumMapping();
            CreateMap<Commands.Game, Core.Domain.Game>().ConvertUsingEnumMapping();
        }
    }
}
