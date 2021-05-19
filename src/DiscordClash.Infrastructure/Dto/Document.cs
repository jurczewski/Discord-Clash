using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DiscordClash.Infrastructure.Dto
{
    public abstract class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
