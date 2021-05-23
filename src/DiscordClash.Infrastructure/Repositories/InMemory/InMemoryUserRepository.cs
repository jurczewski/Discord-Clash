using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordClash.Infrastructure.Repositories.InMemory
{
    public class InMemoryUserRepository : InMemoryGenericRepository<User>, IUserRepository
    {
        public async Task<User> GetByDiscordId(string discordId)
            => await Task.FromResult(_collection.SingleOrDefault(x => x.DiscordId == discordId));
    }
}
