using System;

namespace DiscordClash.Application.Commands
{
    public class SignUpToEvent : ICommand
    {
        public UserCmd User { get; set; }
        public Guid EventId { get; set; }
        public uint Choice { get; set; }
    }

    public class UserCmd : ICommand
    {
        public string DiscordId { get; set; }
        public string DiscordNickname { get; set; }
    }
}
