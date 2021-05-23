using DiscordClash.Core.Domain;
using System.Threading.Tasks;

namespace DiscordClash.Core.Repositories
{
    public interface IEventRepository
    {
        Task<Event> GetByMessageId(ulong discordMessageId);
    }
}
