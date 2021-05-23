using DiscordClash.Core.Domain;
using System.Threading.Tasks;

namespace DiscordClash.Core.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByDiscordId(string discordId);
    }
}
