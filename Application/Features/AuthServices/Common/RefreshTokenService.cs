using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Features.AuthServices.Common
{
    public class RefreshTokenService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IReferenceDataService referenceDataService;

        public RefreshTokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IReferenceDataService referenceDataService)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.referenceDataService = referenceDataService;
        }

        public async Task<string> GenerateRefreshToken(int userId, string username)
        {
            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Name, username),
                new(JwtRegisteredClaimNames.Jti,jti)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var refreshToken = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );
            var refToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);
            var httpContext = httpContextAccessor.HttpContext;
            string? clientIp = httpContext?.Connection.RemoteIpAddress?.ToString();
            string? userAgent = httpContext?.Request.Headers["User-Agent"].ToString();
            var token = TokenDomain.CreateToken(
                jti,
                refToken,
                userId,
                clientIp,
                userAgent
            );
            await referenceDataService.saveToken(token);
            return refToken;
        }
    }
}
