namespace DiscordClash.Application.UseCases.Recommender.Model
{
    public record EventRating
    {
        public double UserId;
        public double EventId;
        public float Label;
    }
}
