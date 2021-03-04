using System;
using System.Threading.Tasks;

namespace AuthWebApps.AuthServices
{
    /// <summary>
    /// Role management service abstraction.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Set account role.
        /// </summary>
        /// <param name="accountId">Account.</param>
        /// <param name="role">Role</param>
        /// <returns>Returns awaiter</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="role"/> is null.</exception>
        Task SetRoleAsync(int accountId, string role);
        
        /// <summary>
        /// Get account role.
        /// </summary>
        /// <param name="accountId">Account.</param>
        /// <returns>Returns account role or <c>null</c> if nothing was found.</returns>
        Task<string> GetRoleAsync(int accountId);
        
        /// <summary>
        /// Reset account role.
        /// </summary>
        /// <param name="accountId">Account.</param>
        /// <returns>Returns awaiter.</returns>
        Task ResetRoleAsync(int accountId);
    }
}
