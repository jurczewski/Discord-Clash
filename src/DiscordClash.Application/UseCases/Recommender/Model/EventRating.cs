using System;

namespace DiscordClash.Application.UseCases.Recommender.Model
{
    public record EventRating
    {
        public double UserId;
        public double EventId;
        public float Label;

        public Guid UserIdGuid;
        public Guid EventIdGuid;

        public EventRating()
        {
        }

        public EventRating(double userId, double eventId, float label, Guid userIdGuid, Guid eventIdGuid)
        {
            UserId = userId;
            EventId = eventId;
            Label = label;
            UserIdGuid = userIdGuid;
            EventIdGuid = eventIdGuid;
        }
    }
}
