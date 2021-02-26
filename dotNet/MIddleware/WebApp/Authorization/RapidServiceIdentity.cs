using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp.Authorization
{
    public class RapidServiceIdentity : ClaimsIdentity
    {
        public static RapidServiceIdentity Unauthorized = new RapidServiceIdentity();

        private RapidServiceIdentity _current = Unauthorized;

        public string Token { get; }

        private RapidServiceIdentity() { }

        public RapidServiceIdentity(string token) : base(CreateClaims(token), Consts.AuthType)
        {
            Token = token;
        }

        private static IEnumerable<Claim> CreateClaims(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var userToken = ServiceToken.Parse(token);
            List<Claim> list = new List<Claim>
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/token", token),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userToken.UserName, null, userToken.Issuer),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userToken.UserId.ToString(), null, userToken.Issuer),
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration", userToken.ExpirationDate.ToString(CultureInfo.InvariantCulture), "DateTime", userToken.Issuer),
            };
            list.AddRange(userToken.Roles.Select((string role) => new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role, null, userToken.Issuer)));
            return list;
        }
    }
}
