using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using DiscordClash.Application.Commands;
using DiscordClash.Application.Messages;
using DiscordClash.Core.Domain;
using System;

namespace DiscordClash.Application.Mappings
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<CreateNewEvent, NewEvent>();
            CreateMap<CreateNewEvent, Event>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Commands.Game, Messages.Game>().ConvertUsingEnumMapping();
            CreateMap<Commands.Game, Core.Domain.Game>().ConvertUsingEnumMapping();
        }
    }
}
