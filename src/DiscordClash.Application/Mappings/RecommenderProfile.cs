using DiscordClash.Application.Queries;
using DiscordClash.Application.UseCases.Recommender.Model;
using System.Collections.Generic;
using System.Linq;

namespace DiscordClash.Application.Mappings
{
    public static class RecommenderProfile
    {
        public static IEnumerable<EventRating> Map(this IEnumerable<ChoiceDto> src)
        {
            return src.Select(c => c.Map());
        }

        public static EventRating Map(this ChoiceDto src)
        {
            return new()
            {
                EventId = src.EventId.GetHashCode(),
                UserId = src.UserId.GetHashCode(),
                Label = src.Label,
                EventIdGuid = src.EventId,
                UserIdGuid = src.UserId
            };
        }
    }
}
