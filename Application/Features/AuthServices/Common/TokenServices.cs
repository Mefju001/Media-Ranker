using Application.Common.Interfaces;
using Domain.Entity;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Features.AuthServices.Common
{
    public class TokenServices
    {
        private readonly IUnitOfWork context;
        private readonly ITokenRepository tokenRepository;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IReferenceDataService referenceDataService;

        public TokenServices(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IReferenceDataService referenceDataService, IUnitOfWork context, ITokenRepository tokenRepository)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.referenceDataService = referenceDataService;
            this.context = context;
            this.tokenRepository = tokenRepository;
        }
        public string generateAccessToken(Guid id, string username, IReadOnlyCollection<ERole>roles)
        {
            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, id.ToString()),
                new(JwtRegisteredClaimNames.Name, username),
                new(JwtRegisteredClaimNames.Jti,jti)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
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
        public async Task<string> GenerateRefreshToken(Guid userId, string username)
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
            var token = Token.CreateToken(
                jti,
                refToken,
                userId,
                clientIp,
                userAgent
            );
            await referenceDataService.saveToken(token);
            return refToken;
        }
        public async Task<IReadOnlyList<Claim>> ValidateAndGetPrincipalFromRefreshToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            var handler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            ClaimsPrincipal principal;
            try
            {
                var secretKey = configuration["Jwt:Key"];
                var issuer = configuration["Jwt:Issuer"];
                var audience = configuration["Jwt:Audience"];

                if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                {
                    throw new InvalidOperationException("Ustawienia JWT (Key, Issuer, Audience) nie zostały skonfigurowane w appsettings.json.");
                }
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(5)
                };
                principal = handler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                var jti = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
                if (jti == null)
                    return null;
                var dbRefreshToken = await tokenRepository.GetByJtiAsync(jti);
                if (dbRefreshToken == null)
                     return null;
                dbRefreshToken.Revoke();
                await context.CompleteAsync();
                return principal.Claims.ToList().AsReadOnly();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
