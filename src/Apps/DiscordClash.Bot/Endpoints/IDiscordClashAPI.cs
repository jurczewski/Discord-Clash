using DiscordClash.Application.Commands;
using Refit;
using System.Threading.Tasks;

namespace DiscordClash.Bot.Endpoints
{
    public interface IDiscordClashApi
    {
        [Post("/events/sign-up")]
        Task SignUpToEvent(SignUpToEvent cmd);
    }
}
