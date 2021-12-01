using System;
using System.Threading.Tasks;

namespace AuthWebApps.AuthServices
{
    /// <summary>
    /// Account service abstraction.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Registers user and assigns unique account id.
        /// </summary>
        /// <param name="login">User login.</param>
        /// <param name="password">User password.</param>
        /// <returns>Returns account of the created user or <c>null</c> if login already existed.</returns>
        /// <exception cref="ArgumentNullException">Throws when one of the arguments is null.</exception>
        Task<int?> RegisterAsync(string login, string password);
        
        /// <summary>
        /// Login user and returns his account id.
        /// </summary>
        /// <param name="login">User login.</param>
        /// <param name="password">User password.</param>
        /// <returns>Returns user account id or <c>null</c> if user wasn't found or password is invalid.</returns>
        Task<int?> LoginAsync(string login, string password);

        /// <summary>
        /// Remove account.
        /// </summary>
        /// <param name="accountId">User account id.</param>
        /// <returns>Returns awaiter.</returns>
        Task<bool> RemoveAsync(int accountId);
    }
}
