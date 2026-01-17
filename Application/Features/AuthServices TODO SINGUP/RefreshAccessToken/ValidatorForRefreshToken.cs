using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Features.AuthServices.RefreshAccessToken
{
    public class ValidatorForRefreshToken
    {
        private readonly IUnitOfWork _context;
        private readonly IConfiguration _configuration;
        private readonly AccessTokenService accessTokenService;
        private readonly RefreshTokenService refreshTokenService;

        public ValidatorForRefreshToken(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _context = unitOfWork;
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
                var secretKey = _configuration["Jwt:Key"];
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];

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
               // var dbRefreshToken = await _context.Tokens.FirstOrDefaultAsync(t => t.Jti == jti.Value && !t.IsRevoked && t.ExpiryDate >= DateTime.UtcNow);
               /* if (dbRefreshToken == null)
                    return null;
                dbRefreshToken.IsRevoked = true;
                dbRefreshToken.RevokedAt = DateTime.UtcNow;*/
                await _context.CompleteAsync();
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
