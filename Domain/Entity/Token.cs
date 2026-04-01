using Domain.Base;

namespace Domain.Entity;

public class Token:AggregateRoot<string>
{
    public string RefreshToken { get; private set; } = default!;
    public Guid UserId { get; init; }
    public DateTime IssuedAt { get; init; }
    public DateTime ExpiryDate { get; init; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? CreatedByIp { get; init; }
    public string? UserAgent { get; init; }


    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
    public bool IsActive => !IsRevoked && !IsExpired;

    private Token() { }

    public static Token Create(string jti, string token, Guid userId, string? ip, string? ua, int daysToExpiry = 7)
    {
        if (string.IsNullOrWhiteSpace(jti)) throw new ArgumentException("JTI is required.");

        return new Token
        {
            Id = jti,
            RefreshToken = token,
            UserId = userId,
            CreatedByIp = ip,
            UserAgent = ua,
            IssuedAt = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(daysToExpiry),
            IsRevoked = false
        };
    }

    public void Revoke()
    {
        if (IsRevoked) return;
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
    }
}