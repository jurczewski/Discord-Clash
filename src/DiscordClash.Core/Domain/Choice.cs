using System;

namespace DiscordClash.Core.Domain
{
    public class Choice : Entity
    {
        public Guid EventId { get; protected set; }
        public Guid UserId { get; protected set; }
        public uint Label { get; protected set; }
    }
}
