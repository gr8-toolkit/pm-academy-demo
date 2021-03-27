using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthWebApps.AuthServices;
using AuthWebApps.StandardScheme.Jwt.Contracts;
using AuthWebApps.StandardScheme.Jwt.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthWebApps.StandardScheme.Jwt.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accounts;
        private readonly IRoleService _roles;
        private readonly JwtOptions _jwt;

        public AuthController(
            IAccountService accounts,
            IRoleService roles,
            IOptions<JwtOptions> jwt)
        {
            _accounts = accounts;
            _roles = roles;
            _jwt = jwt.Value;
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
            
            return await LoginInternalAsync(account);
        }

        /// <summary>
        /// Login action.
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <returns>Returns session id.</returns>
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAsync(AuthRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Demo note: next logic should be moved to dedicated service.
            var account = await _accounts.LoginAsync(request.Login, request.Password);
            return await LoginInternalAsync(account);
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

        private async Task<ActionResult> LoginInternalAsync(int? account)
        {
            if (!account.HasValue) return Unauthorized("Invalid login or password");
            var role = await _roles.GetRoleAsync(account.Value);

            var identity = GetClaimsIdentity(account.Value, role);

            var now = DateTime.UtcNow;
            var jwtToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(_jwt.LifeTime),
                signingCredentials: new SigningCredentials(_jwt.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Ok(response);
        }

        private static ClaimsIdentity GetClaimsIdentity(int accountId, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, accountId.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
            var claimsIdentity =
                new ClaimsIdentity(
                    claims, 
                    "Token", 
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
