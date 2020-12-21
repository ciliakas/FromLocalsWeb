using FromLocalsToLocals.Contracts.Entities;
using System.Net.Http;

namespace FromLocalsToLocals.Web.Utilities.Jwt
{
    public interface IWebApiClient
    {
        HttpClient GetClient(AppUser user);
    }
}
