using AutoMapper;
using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using DiscordClash.Infrastructure.Config;
using DiscordClash.Infrastructure.Dto;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DiscordClash.Infrastructure.Repositories
{
    public class MongoUserRepository : MongoGenericRepository<User, UserDb>, IUserRepository
    {
        public MongoUserRepository(IOptions<MongoDb> settings, IMapper mapper) : base(settings, mapper)
        {

        }

        public async Task<User> GetByDiscordId(ulong discordId)
        {
            var filter = Builders<UserDb>.Filter.Eq(doc => doc.DiscordId, discordId);
            var dto = await Collection.Find(filter).SingleOrDefaultAsync();
            return Mapper.Map<User>(dto);
        }
    }
}
