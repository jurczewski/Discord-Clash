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
    public class MongoEventRepository : MongoGenericRepository<Event, EventDb>, IEventRepository
    {
        public MongoEventRepository(IOptions<MongoDb> settings, IMapper mapper) : base(settings, mapper)
        {
        }

        public async Task<Event> GetByMessageId(ulong discordMessageId)
        {
            var filter = Builders<EventDb>.Filter.Eq(doc => doc.DiscordMessageId, discordMessageId);
            var dto = await Collection.Find(filter).SingleOrDefaultAsync();
            return Mapper.Map<Event>(dto);
        }
    }
}
