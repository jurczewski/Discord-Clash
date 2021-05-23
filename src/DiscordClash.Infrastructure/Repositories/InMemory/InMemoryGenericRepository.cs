using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordClash.Infrastructure.Repositories.InMemory
{
    public class InMemoryGenericRepository<TDomain> : IGenericRepository<TDomain> where TDomain : Entity
    {
        protected readonly ISet<TDomain> _collection = new HashSet<TDomain>();

        public async Task<TDomain> Get(Guid id)
            => await Task.FromResult(_collection.SingleOrDefault(x => x.Id == id));

        public async Task<IEnumerable<TDomain>> GetAll()
            => await Task.FromResult(_collection);

        public async Task Add(TDomain @event)
        {
            _collection.Add(@event);
            await Task.CompletedTask;
        }

        public async Task Update(TDomain @event)
            => await Task.CompletedTask;

        public async Task Delete(Guid id)
        {
            var @event = _collection.SingleOrDefault(x => x.Id == id);
            _collection.Remove(@event);
            await Task.CompletedTask;
        }
    }
}
