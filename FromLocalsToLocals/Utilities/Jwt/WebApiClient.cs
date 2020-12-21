using FromLocalsToLocals.Contracts.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FromLocalsToLocals.Web.Utilities.Jwt
{
    public class WebApiClient : IWebApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public WebApiClient(IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public HttpClient GetClient(AppUser user)
        {
            var client = new HttpClient();

            var token = _tokenService.GetBearerToken(user);

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            client.BaseAddress = new Uri(_configuration["WebApiHost"]);

            return client;
        }
    }
}
