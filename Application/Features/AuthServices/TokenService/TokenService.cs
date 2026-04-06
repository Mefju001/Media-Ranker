using Domain.Entity;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Features.AuthServices.Common
{
    public class TokenService:ITokenService
    {
        private readonly JwtSettings jwtSettings;
        private readonly ITokenRepository tokenRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<TokenService> logger;

        public TokenService(IOptions<JwtSettings> jwtSettings, ILogger<TokenService> tokenServices, IHttpContextAccessor httpContextAccessor, ITokenRepository tokenRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.tokenRepository = tokenRepository;
            this.logger = tokenServices;
            this.jwtSettings = jwtSettings.Value;
        }
        private string CreateToken(IEnumerable<Claim> claims, string secretKey, int expirationMinutes)
        {
            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateAccessToken(Guid id, string username, IReadOnlyCollection<string> roles)
        {
            if (id == Guid.Empty) throw new ArgumentException("User ID cannot be empty.");
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Username cannot be empty.");
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, id.ToString()),
                new(ClaimTypes.Name, username),
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            return CreateToken(claims,jwtSettings.Key,jwtSettings.AccessTokenExpirationMinutes);
        }

        //Opaque token implementation could be added here in the future if needed, currently we are using JWT for both access and refresh tokens for simplicity.
        public async Task<string> GenerateRefreshToken(Guid userId, string username, CancellationToken cancellationToken)
        {
            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, username),
                new(JwtRegisteredClaimNames.Jti,jti)
            };
            var refToken = CreateToken(claims,jwtSettings.Key2,jwtSettings.RefreshTokenExpirationDays*24*60);
            var httpContext = httpContextAccessor.HttpContext;
            var token = Token.Create(
                jti,
                refToken,
                userId,
                httpContext?.Connection.RemoteIpAddress?.ToString(),
                httpContext?.Request.Headers["User-Agent"].ToString()
            );
            await tokenRepository.SaveToken(token, cancellationToken);
            return refToken;
        }
        public async Task<IReadOnlyList<Claim>> ValidateAndGetPrincipalFromRefreshToken(string token, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidCredentialsException("Refresh token is missing.");
            }
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key2)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                var principal = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (validatedToken is not JwtSecurityToken jwtToken || 
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    logger.LogWarning("Niepoprawny algorytm tokena.");
                    throw new InvalidCredentialsException("Invalid token format.");
                }
                var jti = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
                if (jti == null)
                    throw new InvalidCredentialsException("Invalid token claims");
                var dbRefreshToken = await tokenRepository.GetByJtiAsync(jti, cancellationToken);
                if (dbRefreshToken.IsRevoked)
                {
                    logger.LogCritical("WYKRYTO ATAK. Token jti:{jti} był unieważniony.", jti);
                    throw new InvalidCredentialsException("Token has been compromised.");
                }
                if (dbRefreshToken == null || dbRefreshToken.ExpiryDate < DateTime.UtcNow)
                {
                    logger.LogWarning("Token JTI: {Jti} jest nieważny, lub nie istnieje.", jti);
                    throw new InvalidCredentialsException("Refresh token does not exist or expired.");
                }
                dbRefreshToken.Revoke();
                return principal.Claims.ToList().AsReadOnly();
            }
            catch (SecurityTokenExpiredException)
            {
                throw new InvalidCredentialsException("Your session has expired. Please log in again.");
            }
            catch (SecurityTokenException ex)
            {
                logger.LogError(ex, "JWT Security error.");
                throw new InvalidCredentialsException("Invalid security token.");
            }
            catch (Exception ex) when (ex is not InvalidCredentialsException)
            {
                logger.LogError(ex, "Unexpected error during token validation.");
                throw;
            }
        }
    }
}
