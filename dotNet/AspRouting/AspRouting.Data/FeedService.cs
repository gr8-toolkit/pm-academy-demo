using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspRouting.Data
{
    public class FeedService : IFeedService
    {
        // Thread Unsafe !
        private static readonly Dictionary<int, string> FeedItems = new Dictionary<int, string>
        {
            // Seed data
            {1, "Dynamo - Shakhtar" },
            {2, "Vorskla - Metalist" },
            {3, "Zorya - Dymano" },
            {4, "Alexandria - Dynamo" },
            {5, "Zorya - Shakhtar" },
        };
        
        public Task<string> GetItemAsync(int id)
        {
            return Task.FromResult(FeedItems.TryGetValue(id, out var item) ? item : null);
        }

        public Task<IEnumerable<string>> GetItemsAsync()
        {
            return Task.FromResult(FeedItems.Select(item => $"[{item.Key}] {item.Value}"));
        }

        public Task<int> AddItemAsync(string feedItem)
        {
            var id = new Random().Next();
            // TAP violation : possible key exception should be raised in task
            FeedItems.Add(id, feedItem);
            return Task.FromResult(id);
        }

        public Task AddOrUpdateItemAsync(int id, string feedItem)
        {
            FeedItems[id] = feedItem;
            return Task.CompletedTask;
        }

        public Task<bool> DeleteItemAsync(int id)
        {
            return Task.FromResult(FeedItems.Remove(id));
        }

        public Task ClearAsync()
        {
            FeedItems.Clear();
            return Task.CompletedTask;
        }
    }
}
