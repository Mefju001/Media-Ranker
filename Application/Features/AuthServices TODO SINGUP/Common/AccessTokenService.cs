using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApplication1.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;


namespace Application.Features.AuthServices.Common
{
    public class AccessTokenService
    {
        private readonly IConfiguration configuration;

        public AccessTokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string generateAccessToken(int id, string username, ICollection<UserRole> roles)
        {
            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, id.ToString()),
                new(JwtRegisteredClaimNames.Name, username),
                new(JwtRegisteredClaimNames.Jti,jti)
            };
            foreach (var role in roles.Select(ur => ur.Role.role.ToString()))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }
    }
}
