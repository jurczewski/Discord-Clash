namespace DiscordClash.Application.Commands
{
    public class SignUpToEvent : ICommand
    {
        public UserCmd User { get; set; }
        public ulong EventMsgId { get; set; }
        public uint Choice { get; set; }
    }

    public class UserCmd : ICommand
    {
        public ulong DiscordId { get; set; }
        public string DiscordNickname { get; set; }
    }
}
