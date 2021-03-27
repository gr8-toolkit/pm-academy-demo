using System;
using System.Threading.Tasks;
using AuthWebApps.AuthServices;
using AuthWebApps.CustomScheme.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebApps.CustomScheme.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accounts;
        private readonly IRoleService _roles;
        private readonly ISessionService<object> _sessions;

        public AuthController(
            IAccountService accounts,
            IRoleService roles,
            ISessionService<object> sessions)
        {
            _accounts = accounts;
            _roles = roles;
            _sessions = sessions;
        }

        /// <summary>
        /// Registration action.
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <returns>Returns session id.</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> RegisterAsync(AuthRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Demo note: next logic should be moved to dedicated service.

            var account = await _accounts.RegisterAsync(request.Login, request.Password);
            if (!account.HasValue) return Conflict("Login in use");

            // Demo note: kind of abstraction leak;
            // first user - always becomes admin (for demo purposes).
            var role = account <= 1 ? Roles.Admin : Roles.User;
            await _roles.SetRoleAsync(account.Value, role);
            
            account = await _accounts.LoginAsync(request.Login, request.Password);
            if (!account.HasValue) throw new InvalidOperationException("Can't login to newly created account");

            var sessionId = await _sessions.SetSessionsAsync(account.Value, null, DateTime.UtcNow.AddHours(1d));
            return sessionId;
        }

        /// <summary>
        /// Login action.
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <returns>Returns session id.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> LoginAsync(AuthRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Demo note: next logic should be moved to dedicated service.
            var account = await _accounts.LoginAsync(request.Login, request.Password);
            if (!account.HasValue) return Unauthorized("Invalid login or password");
            
            var sessionId = await _sessions.SetSessionsAsync(account.Value, null, DateTime.UtcNow.AddHours(1d));
            return sessionId;
        }

        /// <summary>
        /// Delete account action.
        /// For admin role only.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Return action result.</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("account/{accountId}")]
        public async Task<ActionResult> DeleteAccountAsync(int accountId)
        {
            // Demo note: next logic should be moved to dedicated service.
            if (!await _accounts.RemoveAsync(accountId))
            {
                return NotFound("User with given account id was not found");
            }
            await _sessions.CloseSessionAsync(accountId);
            await _roles.ResetRoleAsync(accountId);
            return NoContent();
        }
    }
}
