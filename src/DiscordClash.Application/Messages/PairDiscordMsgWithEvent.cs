using System;

namespace DiscordClash.Application.Messages
{
    public record PairDiscordMsgWithEvent : IMessage
    {
        public Guid EventId { get; set; }
        public ulong DiscordMsgId { get; set; }
    }
}
