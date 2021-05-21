namespace DiscordClash.Infrastructure.Dto
{
    [BsonCollection("users")]
    public class UserDb : Document
    {
        public string DiscordId { get; set; }
        public string DiscordNickname { get; set; }
    }
}
