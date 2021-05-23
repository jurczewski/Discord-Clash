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
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordClash.Infrastructure.Repositories
{
    /// <summary>
    /// Mongo implementation of generic repository pattern.
    /// </summary>
    /// <typeparam name="TDomain">Domain object, inheriting from Entity object.</typeparam>
    /// <typeparam name="TDtoDb">Dto for DataBase.</typeparam>
    public class MongoGenericRepository<TDomain, TDtoDb> : IGenericRepository<TDomain> where TDomain : Entity where TDtoDb : Document
    {
        protected readonly IMongoCollection<TDtoDb> Collection;
        protected readonly IMapper Mapper;

        public MongoGenericRepository(IOptions<MongoDb> settings, IMapper mapper)
        {
            Mapper = mapper;
            var database = new MongoClient(settings.Value.ConnectionString).GetDatabase(settings.Value.DatabaseName);
            Collection = database.GetCollection<TDtoDb>(GetCollectionName(typeof(TDtoDb)));
        }

        private static string GetCollectionName(ICustomAttributeProvider documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public async Task<TDomain> Get(Guid id)
        {
            var filter = Builders<TDtoDb>.Filter.Eq(doc => doc.Id, id);
            var dto = await Collection.Find(filter).SingleOrDefaultAsync();
            return Mapper.Map<TDomain>(dto);
        }

        public async Task<IEnumerable<TDomain>> GetAll()
        {
            var dtos = Collection.AsQueryable().ToEnumerable();
            return await Task.FromResult(Mapper.Map<IEnumerable<TDomain>>(dtos));
        }

        public async Task Add(TDomain @event)
        {
            var dto = Mapper.Map<TDtoDb>(@event);
            await Collection.InsertOneAsync(dto);
        }

        public async Task Update(TDomain @event)
        {
            var dto = Mapper.Map<TDtoDb>(@event);
            var filter = Builders<TDtoDb>.Filter.Eq(doc => doc.Id, dto.Id);
            await Collection.FindOneAndReplaceAsync(filter, dto);
        }

        public async Task Delete(Guid id)
        {
            var filter = Builders<TDtoDb>.Filter.Eq(doc => doc.Id, id);
            await Collection.FindOneAndDeleteAsync(filter);
        }
    }
}
