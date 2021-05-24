using DiscordClash.Application.Queries;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordClash.Recommender.Endpoints
{
    public interface IDiscordClashApi
    {
        [Get("/choices/")]
        Task<IEnumerable<ChoiceDto>> GetAllChoices();
    }
}
