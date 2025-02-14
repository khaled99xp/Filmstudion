using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Filmstudion.API.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Filmstudion.API.Helpers
{

    public static class JwtHelper
    {

         public static string GenerateJwtToken(User user, IConfiguration configuration)
         {
              var jwtSettings = configuration.GetSection("JwtSettings");
              var secretKey = jwtSettings["Secret"];
              var issuer = jwtSettings["Issuer"];
              var audience = jwtSettings["Audience"];
              var tokenExpiryMinutes = int.Parse(jwtSettings["TokenExpiryMinutes"]);

              var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
              var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

              var claims = new[]
              {
                  new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                  new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                  new Claim(ClaimTypes.Role, user.Role),
                  new Claim("FilmStudioId", user.FilmStudioId.HasValue ? user.FilmStudioId.Value.ToString() : "0"),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
              };

              var token = new JwtSecurityToken(
                  issuer: issuer,
                  audience: audience,
                  claims: claims,
                  expires: DateTime.UtcNow.AddMinutes(tokenExpiryMinutes),
                  signingCredentials: credentials);

              return new JwtSecurityTokenHandler().WriteToken(token);
         }
    }
}
