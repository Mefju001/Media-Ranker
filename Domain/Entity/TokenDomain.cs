using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class TokenDomain
    {
        public string Jti { get; private set; } = string.Empty;
        public string refreshToken { get; private set; } = string.Empty;
        public int UserId { get; private set; }
        public DateTime IssuedAt { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsRevoked { get; private set; } = false;
        public DateTime? RevokedAt { get; private set; }
        public string? CreatedByIp { get; private set; }
        public string? UserAgent { get; private set; }
        //public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
        //public bool IsActive => !IsRevoked && !IsExpired;
        private TokenDomain() { }
        private TokenDomain(string jti, string refreshToken, int userId, string? createdByIp, string? userAgent)
        {
            Jti = jti;
            this.refreshToken = refreshToken;
            UserId = userId;
            CreatedByIp = createdByIp;
            UserAgent = userAgent;
            IssuedAt = DateTime.UtcNow;
            ExpiryDate = DateTime.UtcNow.AddDays(7);
            IsRevoked = false;
        }

        public static TokenDomain CreateToken(string jti, string token, int userId, string? createdByIp, string? userAgent, int daysToExpiry = 7)
        {
            if (string.IsNullOrWhiteSpace(jti)) throw new ArgumentException("JTI cannot be empty");
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException("Token cannot be empty");
            if (userId <= 0) throw new ArgumentException("Invalid UserId");

            return new TokenDomain(jti, token, userId, createdByIp, userAgent);
        }
        public void Revoke()
        {
            if (!IsRevoked)
            {
                IsRevoked = true;
                RevokedAt = DateTime.UtcNow;
            }
        }
    }
}
