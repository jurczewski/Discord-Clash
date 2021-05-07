using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordClash.Infrastructure.Repositories.InMemory
{
    public class InMemoryEventRepository : IEventRepository
    {
        private readonly ISet<Event> _events = new HashSet<Event>();

        public async Task<Event> Get(Guid id)
            => await Task.FromResult(_events.SingleOrDefault(x => x.Id == id));

        public async Task<IEnumerable<Event>> GetAll()
            => await Task.FromResult(_events);

        public async Task Add(Event @event)
        {
            _events.Add(@event);
            await Task.CompletedTask;
        }

        public async Task Update(Event @event)
            => await Task.CompletedTask;

        public async Task Delete(Guid id)
        {
            var @event = _events.SingleOrDefault(x => x.Id == id);
            _events.Remove(@event);
            await Task.CompletedTask;
        }
    }
}
