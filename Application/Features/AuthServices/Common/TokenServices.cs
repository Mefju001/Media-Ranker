using Application.Common.Interfaces;
using Domain.Entity;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TokenServices> logger;

        public TokenServices(ILogger<TokenServices> tokenServices, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IReferenceDataService referenceDataService, IUnitOfWork context, ITokenRepository tokenRepository)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.referenceDataService = referenceDataService;
            this.context = context;
            this.tokenRepository = tokenRepository;
            this.logger = tokenServices;
        }
        public string generateAccessToken(Guid id, string username, IReadOnlyCollection<ERole> roles)
        {
            if (id == Guid.Empty) throw new ArgumentException("User ID cannot be empty.");
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Username cannot be empty.");
            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, id.ToString()),
                new(ClaimTypes.Name, username),
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

        //Opaque token implementation could be added here in the future if needed, currently we are using JWT for both access and refresh tokens for simplicity.
        public async Task<string> GenerateRefreshToken(Guid userId, string username)
        {
            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, username),
                new(JwtRegisteredClaimNames.Jti,jti)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key2"]!));
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
            try
            {
                var secretKey = configuration["Jwt:Key2"];
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
                    ClockSkew = TimeSpan.Zero
                };
                var principal = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (validatedToken is not JwtSecurityToken jwtToken || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    logger.LogWarning("Niepoprawny algorytm tokena.");
                    return null;
                }
                var jti = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
                if (jti == null)
                    return null;
                var dbRefreshToken = await tokenRepository.GetByJtiAsync(jti);
                if (dbRefreshToken == null || dbRefreshToken.IsRevoked || dbRefreshToken.ExpiryDate < DateTime.UtcNow)
                {
                    logger.LogWarning("Token JTI: {Jti} jest nieważny, unieważniony lub nie istnieje.", jti);
                    return null;
                }
                dbRefreshToken.Revoke();
                await context.CompleteAsync();
                return principal.Claims.ToList().AsReadOnly();
            }
            catch (SecurityTokenExpiredException)
            {
                logger.LogInformation("Próba użycia wygasłego Refresh Tokena.");
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Błąd podczas walidacji tokenu odświeżania.");
                return null;
            }
        }
    }
}
