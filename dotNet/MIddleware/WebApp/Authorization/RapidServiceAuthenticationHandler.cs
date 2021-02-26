using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebApp.Authorization
{
    public class RapidServiceAuthenticationHandler : AuthenticationHandler<RapidAuthenticationOptions>
    {
        private readonly ILogger<RapidServiceAuthenticationHandler> _logger;

        public RapidServiceAuthenticationHandler(IOptionsMonitor<RapidAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _logger = logger.CreateLogger<RapidServiceAuthenticationHandler>();
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
                    var claims = new ClaimsPrincipal(new RapidServiceIdentity(token));
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
