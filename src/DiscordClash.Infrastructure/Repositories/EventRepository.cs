using AutoMapper;
using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using DiscordClash.Infrastructure.Config;
using DiscordClash.Infrastructure.Dto;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordClash.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IMongoCollection<EventDb> _events;
        private readonly IMapper _mapper;

        public EventRepository(IOptions<MongoDb> settings, IMapper mapper)
        {
            _mapper = mapper;
            var database = new MongoClient(settings.Value.ConnectionString).GetDatabase(settings.Value.DatabaseName);
            _events = database.GetCollection<EventDb>(GetCollectionName(typeof(EventDb)));
        }

        private static string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public async Task<Event> Get(Guid id)
        {
            var filter = Builders<EventDb>.Filter.Eq(doc => doc.Id, id);
            var dto = await _events.Find(filter).SingleOrDefaultAsync();
            return _mapper.Map<Event>(dto);
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            var dtos = _events.AsQueryable().ToEnumerable();
            return await Task.FromResult(_mapper.Map<IEnumerable<Event>>(dtos));
        }

        public async Task Add(Event @event)
        {
            var dto = _mapper.Map<EventDb>(@event);
            await _events.InsertOneAsync(dto);
        }

        public async Task Update(Event @event)
        {
            var dto = _mapper.Map<EventDb>(@event);
            var filter = Builders<EventDb>.Filter.Eq(doc => doc.Id, dto.Id);
            await _events.FindOneAndReplaceAsync(filter, dto);
        }

        public async Task Delete(Guid id)
        {
            var filter = Builders<EventDb>.Filter.Eq(doc => doc.Id, id);
            await _events.FindOneAndDeleteAsync(filter);
        }
    }
}
