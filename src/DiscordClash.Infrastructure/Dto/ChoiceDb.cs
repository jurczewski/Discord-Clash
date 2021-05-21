using System;

namespace DiscordClash.Infrastructure.Dto
{
    [BsonCollection("choices")]
    public class ChoiceDb : Document
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public uint Label { get; set; }
    }
}
