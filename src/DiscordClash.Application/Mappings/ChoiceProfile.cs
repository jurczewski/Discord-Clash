using System;
using AutoMapper;
using DiscordClash.Application.Commands;
using DiscordClash.Core.Domain;

namespace DiscordClash.Application.Mappings
{
    public class ChoiceProfile : Profile
    {
        public ChoiceProfile()
        {
            CreateMap<SignUpToEvent, Choice>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
            //.ForMember(dest => dest.Id, config => config.MapFrom(src => Guid.NewGuid()));
        }
    }
}
