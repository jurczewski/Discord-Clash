namespace DiscordClash.Recommender.Model
{
    public record EventRating
    {
        public double UserId;
        public double EventId;
        public float Label;
    }
}
