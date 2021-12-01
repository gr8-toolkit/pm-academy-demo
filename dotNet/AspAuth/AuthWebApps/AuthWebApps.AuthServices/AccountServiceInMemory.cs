using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthWebApps.AuthServices.Models;

namespace AuthWebApps.AuthServices
{
    /// <summary>
    /// Implements <see cref="IAccountService"/>.
    /// </summary>
    internal class AccountServiceInMemory : IAccountService
    {
        private readonly ConcurrentDictionary<string, Account> _accounts = new ConcurrentDictionary<string, Account>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <inheritdoc/>
        public async Task<int?> RegisterAsync(string login, string password)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            if (password == null) throw new ArgumentNullException(nameof(password));
            
            var release = await _semaphore.WaitAsync(1000);
            try
            {
                var id = _accounts.Count + 1;
                if (!_accounts.TryAdd(login, new Account(id, password.GetHashCode()))) return null;
                return id;
            }
            finally
            {
                if (release) _semaphore.Release();
            }
        }

        /// <inheritdoc/>
        public Task<int?> LoginAsync(string login, string password)
        {
            if (login == null || password == null) return Task.FromResult<int?>(default);

            if (!_accounts.TryGetValue(login, out var account) || account.PasswordHash != password.GetHashCode())
            {
                return Task.FromResult<int?>(default);
            }

            return Task.FromResult<int?>(account.Id);
        }

        public Task<bool> RemoveAsync(int accountId)
        {
            var login = _accounts.ToArray().FirstOrDefault(a => a.Value.Id == accountId).Key;
            var removed =  login != null && _accounts.TryRemove(login, out _);
            return Task.FromResult(removed);
        }
    }
}
