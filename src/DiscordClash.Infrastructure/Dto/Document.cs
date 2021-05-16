using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DiscordClash.Infrastructure.Dto
{
    public abstract class Document
    {
        [BsonId]
        [BsonElement("id")]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
    }
}
