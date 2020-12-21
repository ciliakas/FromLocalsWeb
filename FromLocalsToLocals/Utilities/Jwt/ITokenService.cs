using FromLocalsToLocals.Contracts.Entities;
using System.Security.Principal;

namespace FromLocalsToLocals.Web.Utilities.Jwt
{
    public interface ITokenService
    {
        string GetBearerToken(AppUser user);
    }
}
