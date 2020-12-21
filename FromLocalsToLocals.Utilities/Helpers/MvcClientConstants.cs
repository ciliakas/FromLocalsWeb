using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FromLocalsToLocals.Utilities.Helpers
{
    public class MvcClientConstants
    {
        public const string Issuer = "MVC";
        public const string Audience = "ApiUser";
        public const string Key = "16charactersneeded";

        public const string AuthSchemes = "Identity.Application" + "," + JwtBearerDefaults.AuthenticationScheme;
    }
}
