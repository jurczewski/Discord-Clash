using System;

namespace DiscordClash.Application.Queries
{
    public record ChoiceDto
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public uint Label { get; set; }
    }
}
