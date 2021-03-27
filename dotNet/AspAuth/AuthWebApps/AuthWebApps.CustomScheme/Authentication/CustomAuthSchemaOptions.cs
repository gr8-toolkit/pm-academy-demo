using Microsoft.AspNetCore.Authentication;

namespace AuthWebApps.CustomScheme.Authentication
{
    public class CustomAuthSchemaOptions : AuthenticationSchemeOptions
    {
        public CustomAuthSchemaOptions()
        {
            ClaimsIssuer = CustomAuthSchema.Issuer;
        }
    }

}
