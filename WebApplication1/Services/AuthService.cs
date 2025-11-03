using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Data;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Exceptions;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(IConfiguration config, AppDbContext context, IPasswordHasher<User> passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = config;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<TokenResponse?> Login(LoginRequest loginRequest)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.username == loginRequest.username);
            if (user == null)
            {
                return null;
            }
            var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.password, loginRequest.password);
            if (passwordVerification == PasswordVerificationResult.Failed)
            {
                return null;
            }
            var accessToken = GenerateAccessToken(user.Id, loginRequest.username, user.UserRoles);
            var refreshToken = await GenerateRefreshToken(user.Id, loginRequest.username);
            return new TokenResponse(accessToken, refreshToken);
        }
        private string GenerateAccessToken(int id, string username, ICollection<UserRole> roles)
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }
        private async Task<IReadOnlyList<Claim>> ValidateAndGetPrincipalFromRefreshToken(string token)
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
                    throw new InvalidOperationException("Ustawienia JWT (Key, Issuer, Audience) nie zosta³y skonfigurowane w appsettings.json.");
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
                var dbRefreshToken = await _context.Tokens.FirstOrDefaultAsync(t => t.Jti == jti.Value && !t.IsRevoked && t.ExpiryDate >= DateTime.UtcNow);
                if (dbRefreshToken == null)
                    return null;
                dbRefreshToken.IsRevoked = true;
                dbRefreshToken.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return principal.Claims.ToList().AsReadOnly();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        private async Task<IReadOnlyList<Claim>> ValidateAndGetPrincipalFromToken(string token)
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
                    throw new InvalidOperationException("Ustawienia JWT (Key, Issuer, Audience) nie zosta³y skonfigurowane w appsettings.json.");
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
                return principal.Claims.ToList().AsReadOnly();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var refreshToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );
            var refToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);
            var httpContext = _httpContextAccessor.HttpContext;
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
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();
            return refToken;
        }
        public async Task<TokenResponse?> RefreshAccessToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new NotFoundException("Not found your user");
            }
            var claims = await ValidateAndGetPrincipalFromRefreshToken(refreshToken);
            var username = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name) ?? throw new UserClaimNotFoundException("Not found your user in claims");
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.username == username.Value.ToString());
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var accessToken = GenerateAccessToken(user.Id, username.Value.ToString(), user.UserRoles);
            var RefreshToken = await GenerateRefreshToken(user.Id, username.Value.ToString());
            return new TokenResponse(accessToken, RefreshToken);
        }
        public /*async*/ Task<TokenResponse?> registerAccount(User user)
        {
            throw new NotImplementedException();
        }
        public async Task Logout(string stringUserId)
        {
            if (!int.TryParse(stringUserId, out var userId))
                throw new Exception("Something go wrong");
            var userRefreshTokens = await _context.Tokens
                .Where(t => t.UserId == userId)
                .ToListAsync() ?? throw new NotFoundException("Not found in db");
            if (userRefreshTokens.Any())
            {
                foreach (var token in userRefreshTokens)
                {
                    token.IsRevoked = true;
                    token.RevokedAt = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
