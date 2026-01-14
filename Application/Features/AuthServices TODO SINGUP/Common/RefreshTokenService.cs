using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Domain.Entities;
using WebApplication1.Services.Interfaces;

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
            var token = new Token()
            {
                Jti = jti,
                refreshToken = refToken,
                UserId = userId,
                IssuedAt = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(7),
                IsRevoked = false,
                RevokedAt = null,
                CreatedByIp = clientIp,
                UserAgent = userAgent
            };
            await referenceDataService.saveToken(token);
            return refToken;
        }
    }
}
