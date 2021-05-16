using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using DiscordClash.Core.Domain;
using DiscordClash.Infrastructure.Dto;

namespace DiscordClash.Infrastructure.Mappings
{
    public class EventDbProfile : Profile
    {
        public EventDbProfile()
        {
            CreateMap<EventDb, Event>().ReverseMap();

            CreateMap<Dto.Game, Core.Domain.Game>().ConvertUsingEnumMapping();
        }
    }
}
