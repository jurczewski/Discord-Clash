using AutoMapper;
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
    //todo: add entity do domain
    public class MongoGenericRepository<TDtoDb, TDomain> : IGenericRepository<TDomain> where TDomain : class where TDtoDb : Document
    {
        private readonly IMongoCollection<TDtoDb> _collection;
        private readonly IMapper _mapper;

        public MongoGenericRepository(IOptions<MongoDb> settings, IMapper mapper)
        {
            _mapper = mapper;
            var database = new MongoClient(settings.Value.ConnectionString).GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<TDtoDb>(GetCollectionName(typeof(TDtoDb)));
        }

        private static string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public async Task<TDomain> Get(Guid id)
        {
            var filter = Builders<TDtoDb>.Filter.Eq(doc => doc.Id, id);
            var dto = await _collection.Find(filter).SingleOrDefaultAsync();
            return _mapper.Map<TDomain>(dto);
        }

        public async Task<IEnumerable<TDomain>> GetAll()
        {
            var dtos = _collection.AsQueryable().ToEnumerable();
            return await Task.FromResult(_mapper.Map<IEnumerable<TDomain>>(dtos));
        }

        public async Task Add(TDomain @event)
        {
            var dto = _mapper.Map<TDtoDb>(@event);
            await _collection.InsertOneAsync(dto);
        }

        public async Task Update(TDomain @event)
        {
            var dto = _mapper.Map<TDtoDb>(@event);
            var filter = Builders<TDtoDb>.Filter.Eq(doc => doc.Id, dto.Id);
            await _collection.FindOneAndReplaceAsync(filter, dto);
        }

        public async Task Delete(Guid id)
        {
            var filter = Builders<TDtoDb>.Filter.Eq(doc => doc.Id, id);
            await _collection.FindOneAndDeleteAsync(filter);
        }
    }
}
