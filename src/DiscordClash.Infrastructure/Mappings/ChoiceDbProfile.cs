using AutoMapper;
using DiscordClash.Core.Domain;
using DiscordClash.Infrastructure.Dto;

namespace DiscordClash.Infrastructure.Mappings
{
    public class ChoiceDbProfile : Profile
    {
        public ChoiceDbProfile()
        {
            CreateMap<ChoiceDb, Choice>().ReverseMap();
        }
    }
}

