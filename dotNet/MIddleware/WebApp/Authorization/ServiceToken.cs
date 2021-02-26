using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Authorization
{
    public class ServiceToken
    {
        public string UserName { get; private set; }

        public string UserId { get; private set; }

        public DateTime ExpirationDate { get; private set; }

        public string Issuer { get; private set; }

        public IEnumerable<string> Roles { get; private set; }

        public static ServiceToken Parse(string token)
        {
            return new ServiceToken
            {
                UserName = "Vaprov (service)",
                UserId = "8",
                ExpirationDate = DateTime.MaxValue,
                Issuer = Consts.Issuer,
                Roles = new List<string>
                {
                    WebApp.Authorization.Roles.Admin,
                    WebApp.Authorization.Roles.Test,
                }
            };
        }
    }
}
