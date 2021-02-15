using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspRouting.Data
{
    public interface IFeedService
    {
        Task<string> GetItemAsync(int id);
        Task<IEnumerable<string>> GetItemsAsync();
        Task<int> AddItemAsync(string feedItem);
        Task AddOrUpdateItemAsync(int id, string feedItem);
        Task<bool> DeleteItemAsync(int id);
        Task ClearAsync();
    }
}
