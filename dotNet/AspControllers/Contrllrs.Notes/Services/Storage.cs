using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contrllrs.Notes.Models;

namespace Contrllrs.Notes.Services
{
    /// <summary>
    /// Thread unsafe simple storage.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public class Storage<T> : IStorage<T> where T : class
    {
        private readonly Dictionary<int, T> _db = new Dictionary<int, T>();

        public IEnumerable<ItemWithId<T>> GetAll()
        {
            return _db.Select(x => new ItemWithId<T> {Id = x.Key, Item = x.Value}).ToArray();
        }

        public Task<IEnumerable<ItemWithId<T>>> GetAllAsync()
        {
            return Task.FromResult(GetAll());
        }

        public T Get(int id)
        {
            return _db.TryGetValue(id, out var item) ? item : default;
        }

        public Task<T> GetAsync(int id)
        {
            return Task.FromResult(Get(id));
        }

        public int Add(T item)
        {
            var id = _db.Keys.Any() ? _db.Keys.Max() + 1 : 1;
            _db.Add(id, item);
            return id;
        }

        public Task<int> AddAsync(T item)
        {
            return Task.FromResult(Add(item));
        }

        public void AddOrUpdate(int id, T item)
        {
            _db[id] = item;
        }

        public Task AddOrUpdateAsync(int id, T item)
        {
            AddOrUpdate(id, item);
            return Task.CompletedTask;
        }

        public bool Delete(int id)
        {
            return  _db.Remove(id);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return Task.FromResult(Delete(id));
        }
    }
}
