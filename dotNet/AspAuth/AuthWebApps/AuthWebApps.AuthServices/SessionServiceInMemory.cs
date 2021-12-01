using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using AuthWebApps.AuthServices.Models;

namespace AuthWebApps.AuthServices
{
    /// <summary>
    /// Implements <see cref="ISessionService{T}"/>.
    /// </summary>
    internal class SessionServiceInMemory<T> : ISessionService<T>
    {
        private readonly ConcurrentDictionary<string, Session<T>> _sessions = new ConcurrentDictionary<string, Session<T>>();

        /// <inheritdoc/>
        public Task<string> SetSessionsAsync(int accountId, T sessionData, DateTime expiresAt)
        {
            var session = new Session<T>(sessionData, accountId, expiresAt);
            var sessionId = Guid.NewGuid().ToString("N");
            
            return _sessions.TryAdd(sessionId, session)
                ? Task.FromResult(sessionId)
                : Task.FromException<string>(new InvalidOperationException("Can't add session"));
        }

        /// <inheritdoc/>
        public Task<Session<T>> GetSessionAsync(string sessionId, int? accountId, DateTime? expiresAt)
        {
            if (sessionId == null) return Task.FromResult<Session<T>>(default);
            
            if (!_sessions.TryGetValue(sessionId, out var session)) return Task.FromResult<Session<T>>(default);
            
            if (session.Expiration < DateTime.UtcNow)
            {
                _sessions.TryRemove(sessionId, out _);
                session = null;
            }

            if (accountId.HasValue && session != null && accountId.Value != session.AccountId)
            {
                session = null;
            }
            
            if (expiresAt.HasValue && session != null)
            {
                // prolongation
                session.Expiration = expiresAt.Value;
            }

            return Task.FromResult(session);
        }

        /// <inheritdoc/>
        public Task CloseSessionAsync(string sessionId)
        {
            if (sessionId!=null)  _sessions.TryRemove(sessionId, out _);
            return Task.CompletedTask;
        }

        public Task CloseSessionAsync(int accountId)
        {
            var sessions = _sessions.ToArray().Where(s => s.Value.AccountId == accountId).Select(s => s.Key).ToList();
            sessions.ForEach(sessionId => _sessions.TryRemove(sessionId, out _));
            return Task.CompletedTask;
        }
    }
}
