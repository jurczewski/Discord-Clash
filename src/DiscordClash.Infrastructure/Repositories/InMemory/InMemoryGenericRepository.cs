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
        protected readonly ISet<TDomain> Collection = new HashSet<TDomain>();

        public async Task<TDomain> Get(Guid id)
            => await Task.FromResult(Collection.SingleOrDefault(x => x.Id == id));

        public async Task<IEnumerable<TDomain>> GetAll()
            => await Task.FromResult(Collection);

        public async Task Add(TDomain @event)
        {
            Collection.Add(@event);
            await Task.CompletedTask;
        }

        public async Task Update(TDomain @event)
            => await Task.CompletedTask;

        public async Task Delete(Guid id)
        {
            var @event = Collection.SingleOrDefault(x => x.Id == id);
            Collection.Remove(@event);
            await Task.CompletedTask;
        }
    }
}
