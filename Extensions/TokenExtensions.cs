using System.IdentityModel.Tokens.Jwt;
using HungrAPI.Constants;
using HungrAPI.Models;

namespace HungrAPI.Extensions;

public static class TokenExtensions
{
    public static User? GetUser(this string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var email = securityToken.Claims.FirstOrDefault(c => c.Type == JwtClaims.Email)?.Value;
            var name = securityToken.Claims.FirstOrDefault(c => c.Type == JwtClaims.Name)?.Value;

            return new User
            {
                GoogleId = securityToken.Subject,
                Name = name,
                Email = email
            };
        }
        catch (Exception)
        {
            return null;
        }
    }
}