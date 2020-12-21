using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Utilities.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FromLocalsToLocals.Web.Utilities.Jwt
{
    public class TokenService : ITokenService
    {
        public string GetBearerToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MvcClientConstants.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            var token = new JwtSecurityToken(
                MvcClientConstants.Issuer,
                MvcClientConstants.Audience,
                claims,
                expires : DateTime.UtcNow.AddMinutes(1),
                signingCredentials : creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
