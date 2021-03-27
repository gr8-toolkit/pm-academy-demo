using System;
using System.Threading.Tasks;
using AuthWebApps.AuthServices.Models;

namespace AuthWebApps.AuthServices
{
    /// <summary>
    /// Session service abstraction.
    /// </summary>
    /// <typeparam name="TSessionData">Session data type.</typeparam>
    public interface ISessionService<TSessionData>
    {
        /// <summary>
        /// Create session with data.
        /// </summary>
        /// <param name="accountId">Session owner.</param>
        /// <param name="sessionData">Session data.</param>
        /// <param name="expiresAt">Session expiration absolute time.</param>
        /// <returns>Returns session id.</returns>
        Task<string> SetSessionsAsync(int accountId, TSessionData sessionData, DateTime expiresAt);

        /// <summary>
        /// Get session data.
        /// </summary>
        /// <param name="sessionId">Session id.</param>
        /// <param name="accountId">Optional session account check.</param>
        /// <param name="expiresAt">Optional session prolongation absolute time.</param>
        /// <returns>Returns session data or <c>null</c> if session wasn't found or expired or account is invalid.</returns>
        Task<Session<TSessionData>> GetSessionAsync(string sessionId, int? accountId, DateTime? expiresAt);

        /// <summary>
        /// Close session.
        /// </summary>
        /// <param name="sessionId">Session id.</param>
        /// <returns>Returns awaiter</returns>
        Task CloseSessionAsync(string sessionId);

        /// <summary>
        /// Close session for account.
        /// </summary>
        /// <param name="accountId">Session owner.</param>
        /// <returns>Returns awaiter</returns>
        Task CloseSessionAsync(int accountId);
    }
}
