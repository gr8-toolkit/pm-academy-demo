using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthWebApps.AuthServices;
using AuthWebApps.StandardScheme.Cookies.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebApps.StandardScheme.Cookies.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accounts;
        private readonly IRoleService _roles;

        public AuthController(
            IAccountService accounts,
            IRoleService roles)
        {
            _accounts = accounts;
            _roles = roles;
        }

        /// <summary>
        /// Registration action.
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <returns>Returns session id.</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterAsync(AuthRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Demo note: next logic should be moved to dedicated service.

            var account = await _accounts.RegisterAsync(request.Login, request.Password);
            if (!account.HasValue) return Conflict("Login in use");

            // Demo note: kind of abstraction leak;
            // first user - always becomes admin (for demo purposes).
            var role = account <= 1 ? Roles.Admin : Roles.User;
            
            account = await _accounts.LoginAsync(request.Login, request.Password);
            if (!account.HasValue) throw new InvalidOperationException("Can't login to newly created account");

            await SignInAsync(account.Value, role);
            return Ok();
        }
        
        /// <summary>
        /// Login action.
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <returns>Returns session id.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAsync(AuthRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Demo note: next logic should be moved to dedicated service.
            var account = await _accounts.LoginAsync(request.Login, request.Password);
            if (!account.HasValue) return Unauthorized("Invalid login or password");

            var role = await _roles.GetRoleAsync(account.Value);
            
            await SignInAsync(account.Value, role);
            return Ok();
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
            await _roles.ResetRoleAsync(accountId);
            return NoContent();
        }

        private async Task SignInAsync(int accountId, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, accountId.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
            var account = new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(account));
        }
    }
}
