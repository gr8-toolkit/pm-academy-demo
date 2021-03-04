using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AuthWebApps.AuthServices
{
    internal class RoleServiceInMemory : IRoleService
    {
        private readonly ConcurrentDictionary<int, string> _roles = new ConcurrentDictionary<int, string>();
        
        public Task SetRoleAsync(int accountId, string role)
        {
            _roles[accountId] = role ?? throw new ArgumentNullException(nameof(role));
            return Task.CompletedTask;
        }

        public Task<string> GetRoleAsync(int accountId)
        {
             var existed = _roles.TryGetValue(accountId, out var role);
             return Task.FromResult(existed ? role : null);
        }

        public Task ResetRoleAsync(int accountId)
        {
            _roles.TryRemove(accountId, out _);
            return Task.CompletedTask;
        }
    }
}
