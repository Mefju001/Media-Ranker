using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Exceptions;
using WebApplication1.Infrastructure.Persistence;

namespace WebApplication1.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMediator mediator;

        public AuthService(IMediator mediator, IConfiguration config, AppDbContext context, IPasswordHasher<User> passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = config;
            _httpContextAccessor = httpContextAccessor;
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
