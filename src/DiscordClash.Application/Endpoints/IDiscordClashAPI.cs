using DiscordClash.Application.Commands;
using DiscordClash.Application.Queries;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordClash.Application.Endpoints
{
    public interface IDiscordClashApi
    {
        #region Bot
        [Post("/events/sign-up")]
        Task SignUpToEvent(SignUpToEvent cmd);
        #endregion

        #region recommender

        [Get("/choices/")]
        Task<IEnumerable<ChoiceDto>> GetAllChoices();

        #endregion
    }
}
