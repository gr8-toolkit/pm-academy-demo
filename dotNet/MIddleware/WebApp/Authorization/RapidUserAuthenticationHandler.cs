using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace WebApp.Authorization
{
    public class RapidUserAuthenticationHandler : AuthenticationHandler<RapidAuthenticationOptions>
    {
        private readonly ILogger<RapidUserAuthenticationHandler> _logger;

        public RapidUserAuthenticationHandler(IOptionsMonitor<RapidAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _logger = logger.CreateLogger<RapidUserAuthenticationHandler>();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!TryGetTokenFromHeaders(base.Request, out var token))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            try
            {
                if (base.Options.VerifyToken(token))
                {
                    var claims = new ClaimsPrincipal(new RapidUserIdentity(token));
                    return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claims, Options.ClaimsIssuer)));
                }
                _logger.LogInformation("Verification failed");
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Verification failed with error");
                return Task.FromResult(AuthenticateResult.Fail(e));
            }
        }

        public static bool TryGetTokenFromHeaders(HttpRequest request, out string token)
        {
            string key = "Authorization";
            if (request.Headers.ContainsKey(key))
            {
                string[] array = request.Headers[key].First().Split(' ');
                switch (array.Length)
                {
                    case 2 when array[0] == "Bearer":
                        token = array[1];
                        return true;
                    case 1:
                        token = array[0];
                        return true;
                }
            }
            token = null;
            return false;
        }
    }
}
