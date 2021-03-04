using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Authorization
{
    public class RapidAuthenticationOptions : AuthenticationSchemeOptions
    {
        public Predicate<string> VerifyToken
        {
            get;
            set;
        }

        public RapidAuthenticationOptions()
        {
            ClaimsIssuer = Consts.Issuer;
        }
    }
}
