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
            return src.Select(c => new EventRating
            {
                EventId = c.EventId.GetHashCode(),
                UserId = c.UserId.GetHashCode(),
                Label = c.Label
            });
        }
    }
}
