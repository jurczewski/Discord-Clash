using AutoMapper;
using DiscordClash.Application.Commands;
using DiscordClash.Application.Queries;
using DiscordClash.Core.Domain;
using System;

namespace DiscordClash.Application.Mappings
{
    public class ChoiceProfile : Profile
    {
        public ChoiceProfile()
        {
            CreateMap<SignUpToEvent, Choice>()
                .ForMember(dest => dest.Label, config => config.MapFrom(src => src.Choice))
                .ForMember(dest => dest.Id, config => config.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Choice, ChoiceDto>();
        }
    }
}
