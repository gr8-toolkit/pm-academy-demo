using System.Collections.Generic;
using System.Threading.Tasks;
using Contrllrs.Notes.Models;

namespace Contrllrs.Notes.Services
{
    public interface IStorage<T> where T:class
    {
        IEnumerable<ItemWithId<T>> GetAll();
        Task<IEnumerable<ItemWithId<T>>> GetAllAsync();

        T Get(int id);
        Task<T> GetAsync(int id);

        int Add(T item);
        Task<int> AddAsync(T item);

        void AddOrUpdate(int id, T item);
        Task AddOrUpdateAsync(int id, T item);

        bool Delete(int id);
        Task<bool> DeleteAsync(int id);
    }
}
