using AutoMapper;
using DiscordClash.Core.Domain;
using DiscordClash.Infrastructure.Dto;

namespace DiscordClash.Infrastructure.Mappings
{
    public class UserDbProfile : Profile
    {
        public UserDbProfile()
        {
            CreateMap<UserDb, User>().ReverseMap();
        }
    }
}
