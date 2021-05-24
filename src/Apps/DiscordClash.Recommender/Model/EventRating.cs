namespace DiscordClash.Recommender.Model
{
    public record EventRating
    {
        public double UserId { get; set; }
        public double EventId { get; set; }
        public uint Label { get; set; }
    }
}
