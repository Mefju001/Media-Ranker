using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
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
        private readonly IPasswordHasher<User> _passwordHasher;
        public AuthService(IConfiguration config, AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = config;
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
            var refreshToken = GenerateRefreshToken(user.Id, loginRequest.username);
            return new TokenResponse(accessToken,refreshToken);
        }
        private string GenerateAccessToken(int id, string username, ICollection<UserRole>roles)
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
        private ClaimsPrincipal GetClaimsFromJwt(string token)
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), // Twój tajny klucz
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                if (!handler.CanReadToken(token))
                {
                    return null;
                }
                var readtoken = handler.ReadJwtToken(token);
                principal = handler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        private async Task<Boolean> findRefreshTokenInDb(string refreshToken)
        {
            var claims = GetClaimsFromJwt(refreshToken);
            if (claims == null)
                return false;
            var jti = claims.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
            if (jti == null)
                return false;
            var dbRefreshToken = await _context.Tokens.FirstOrDefaultAsync(t => t.refreshToken == refreshToken&&t.Jti == jti.Value);
            if (dbRefreshToken == null)
                return false;
            if (dbRefreshToken.IsRevoked == true)
                throw new Exception("Token is revoked");
            return true;
        }
        public string GenerateRefreshToken(int userId, string username)
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
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }
        public async Task<TokenResponse?> RefreshAccessToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new NotFoundException("Not found your user");
            }
            var claims = GetClaimsFromJwt(refreshToken);
            var username = claims.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);
            if (username == null)
                throw new UserClaimNotFoundException("Not found your user in claims");
            var finded = findRefreshTokenInDb(refreshToken);
            if (finded.Result is false)
            {
                throw new InvalidRefreshTokenException("Your refresh token is not in db");
            }
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.username == username.Value.ToString());
            if (user == null)
                return new TokenResponse(null, null);
            var accessToken = GenerateAccessToken(user.Id, username.Value.ToString(), user.UserRoles);
            var RefreshToken = GenerateRefreshToken(user.Id, username.Value.ToString());
            return new TokenResponse(accessToken, RefreshToken);
        }
        public async Task<TokenResponse?>registerAccount(User user)
        {
            return new TokenResponse(null, null);
        }
    }
}
