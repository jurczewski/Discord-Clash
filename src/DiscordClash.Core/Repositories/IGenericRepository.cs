using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordClash.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T obj);
        Task Update(T obj);
        Task Delete(Guid id);
    }
}
