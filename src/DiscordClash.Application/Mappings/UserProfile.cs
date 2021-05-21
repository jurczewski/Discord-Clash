using AutoMapper;
using DiscordClash.Application.Commands;
using DiscordClash.Core.Domain;
using System;

namespace DiscordClash.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCmd, User>();
            //.ForMember(dest => dest.Id, config => config.MapFrom(src => Guid.NewGuid()))
            //.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
