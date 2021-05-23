using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordClash.Infrastructure.Repositories.InMemory
{
    public class InMemoryEventRepository : InMemoryGenericRepository<Event>, IEventRepository
    {
        public async Task<Event> GetByMessageId(ulong discordMessageId)
            => await Task.FromResult(Collection.SingleOrDefault(e => e.DiscordMessageId == discordMessageId));
    }
}
