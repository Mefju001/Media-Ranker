using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Common.UserContext
{
    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public Guid? Jti
        {
            get
            {
                var jti = httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Jti);
                return Guid.TryParse(jti, out Guid jtiGuid) ? jtiGuid : null;
            }
        }
        public Guid? UserId
        {
            get
            {
                var id = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return Guid.TryParse(id, out Guid userId) ? userId : null;
            }
        }
        public string? UserName
        {
            get
            {
                var username = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
                return username;
            }
        }
        public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
