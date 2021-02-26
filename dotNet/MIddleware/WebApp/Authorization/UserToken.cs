using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Authorization
{
    public class UserToken
    {
        public string UserName { get; private set; }

        public string UserId { get; private set; }

        public DateTime ExpirationDate { get; private set; }

        public string Issuer { get; private set; }

        public string BrandId { get; private set; }

        public IEnumerable<string> Roles { get; private set; }

        public static UserToken Parse(string token)
        {
            return new UserToken
            {
                UserName = "Vaprov (user)",
                UserId = "8",
                ExpirationDate = DateTime.MaxValue,
                Issuer = Consts.Issuer,
                BrandId = "COM",

                Roles = new List<string>
                {
                    WebApp.Authorization.Roles.User,
                }
            };
        }
    }
}
