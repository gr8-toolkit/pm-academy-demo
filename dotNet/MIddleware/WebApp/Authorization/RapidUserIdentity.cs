using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Globalization;

namespace WebApp.Authorization
{
    public class RapidUserIdentity : ClaimsIdentity
    {
        public static RapidUserIdentity Unauthorized = new RapidUserIdentity();

        private RapidUserIdentity _current = Unauthorized;

        public string Token { get; }

        private RapidUserIdentity() { }

        public RapidUserIdentity(string token) : base(CreateClaims(token), Consts.AuthType)
        {
            Token = token;
        }

        private static IEnumerable<Claim> CreateClaims(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var userToken = UserToken.Parse(token);
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
