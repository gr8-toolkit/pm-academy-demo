using System.Collections.Generic;
using System.Security.Claims;

namespace AuthWebApps.CustomScheme.Authentication
{
    public class AccountIdentity : ClaimsIdentity
    {
        public int AccountId { get; set; }
        public string SessionId { get; set; }
        public string Role { get; set; }

        public AccountIdentity()
        {
        }

        public AccountIdentity(int accountId, string sessionId, string role) 
            : base(CreateClaimsIdentity(accountId, sessionId, role), CustomAuthSchema.Type)
        {
            AccountId = accountId;
            SessionId = sessionId;
            Role = role;
        }

        private static IEnumerable<Claim> CreateClaimsIdentity(
            int accountId,
            string sessionId,
            string role)
        {
            var result = new List<Claim>
            {
                new Claim(DefaultNameClaimType, accountId.ToString()),
            };

            if (sessionId != null)
            {
                result.Add(new Claim("SessionId", sessionId));
            }

            if (role != null)
            {
                result.Add(new Claim(DefaultRoleClaimType, role));
            }

            return result;
        }
    }
}
