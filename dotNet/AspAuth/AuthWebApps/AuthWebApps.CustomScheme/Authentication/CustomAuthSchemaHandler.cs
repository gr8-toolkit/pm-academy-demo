using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AuthWebApps.AuthServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace AuthWebApps.CustomScheme.Authentication
{
    public class CustomAuthSchemaHandler : AuthenticationHandler<CustomAuthSchemaOptions>
    {
        private readonly ISessionService<object> _sessions;
        private readonly IRoleService _roles;

        public CustomAuthSchemaHandler(
            IOptionsMonitor<CustomAuthSchemaOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            ISessionService<object> sessions,
            IRoleService roles) 
            : base(options, logger, encoder, clock)
        {
            _sessions = sessions;
            _roles = roles;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!TryGetSessionIdFromRequest(Request, out var sessionId)) return AuthenticateResult.NoResult();
            try
            {
                var session = await _sessions.GetSessionAsync(sessionId, null, null);
                if (session == null) return AuthenticateResult.NoResult();
                var role = await _roles.GetRoleAsync(session.AccountId);
                return AuthenticateResult.Success(
                    new AuthenticationTicket(
                        new ClaimsPrincipal(new AccountIdentity(session.AccountId, sessionId, role)),
                        CustomAuthSchema.Name));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error during authentication");
                return AuthenticateResult.Fail(ex);
            }
        }
        
        private static bool TryGetSessionIdFromRequest(HttpRequest request, out string sessionId)
        {
            sessionId = null;
            if (request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                sessionId = request.Headers[HeaderNames.Authorization].FirstOrDefault();
            }
            return !string.IsNullOrEmpty(sessionId);
        }
    }
}
