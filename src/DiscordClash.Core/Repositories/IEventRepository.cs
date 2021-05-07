using DiscordClash.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordClash.Core.Repositories
{
    public interface IEventRepository
    {
        Task<Event> Get(Guid id);
        Task<IEnumerable<Event>> GetAll();
        Task Add(Event @event);
        Task Update(Event @event);
        Task Delete(Guid id);
    }
}
