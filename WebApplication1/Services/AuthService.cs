<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Identity;
=======
﻿using Azure;
<<<<<<< HEAD
using Azure.Core;
=======
>>>>>>> e771017e4cbc805d3feaf371132b7f76040c1767
using Microsoft.AspNetCore.Identity;
>>>>>>> 5f6ae51 (update loginController&AuthService)
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
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
        public async Task<TokenResponse?>Login(LoginRequest loginRequest)
        {
            var user = await _context.Users
                .Include(u=>u.UserRoles)
                .ThenInclude(ur=>ur.Role)
                .FirstOrDefaultAsync(u => u.username == loginRequest.username);
            if (user == null)
            {
                return null;
            }
            var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.password, loginRequest.password);
            if(passwordVerification == PasswordVerificationResult.Failed)
            {
                return null;
            }
            var token = GenerateJwtToken(user.Id, loginRequest.username, user.UserRoles);
<<<<<<< HEAD
            return token;
=======
<<<<<<< HEAD
            var refreshToken = GenerateJwtRefreshToken(user.Id,loginRequest.username);
=======
            var refreshToken = GenerateJwtRefreshToken(user.Id);
>>>>>>> e771017e4cbc805d3feaf371132b7f76040c1767
            return (token,refreshToken);
>>>>>>> 5f6ae51 (update loginController&AuthService)
        }
        public TokenResponse GenerateJwtToken(int userId, string username,ICollection<UserRole> roles)
        {
            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
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
            var roleResponses = roles.Select(r => new RoleResponse(r.Role.role)).ToList();
            return new TokenResponse(username, roleResponses, new JwtSecurityTokenHandler().WriteToken(token));
        }
<<<<<<< HEAD
=======
<<<<<<< HEAD
        private IEnumerable<Claim> GetClaimsFromJwt(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Enumerable.Empty<Claim>();
            }
            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (!handler.CanReadToken(token))
                {
                    return Enumerable.Empty<Claim>();
                }
                var readtoken = handler.ReadJwtToken(token);
                return readtoken.Claims;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Enumerable.Empty<Claim>();

            }
        }
        private async Task<Boolean> ValidateRefreshToken(string refreshToken)
        {
            var claims = GetClaimsFromJwt(refreshToken);
            if (claims == null)
                return false;
            var jti = claims.FirstOrDefault(c=>c.Type == JwtRegisteredClaimNames.Jti);
            if (jti==null)
                return false;
            var dbRefreshToken = await _context.Tokens.FirstOrDefaultAsync(t => t.refreshToken == refreshToken);
            if(dbRefreshToken == null)
                return false;
            return true;
        }
        public string GenerateJwtRefreshToken(int userId, string username)
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
        public async Task<(TokenResponse,string?refreshToken)>RefreshAccessToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return (null,null);
            }
            var validated = ValidateRefreshToken(refreshToken);
            if (validated != null)
            {
                //var accessToken = GenerateJwtToken();
                //var refreshToken = GenerateJwtRefreshToken();
            }

            return (null,null);
=======
        public string GenerateJwtRefreshToken(int userId)
        {
            var refreshToken = Guid.NewGuid().ToString();
            return refreshToken;
        }
        public async Task<TokenResponse>RefreshAccessToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            return null;
>>>>>>> e771017e4cbc805d3feaf371132b7f76040c1767
        }
>>>>>>> 5f6ae51 (update loginController&AuthService)
    }
}
