namespace Domain;

public class RefreshToken
{
    public Guid Id { get; private set; } = default!;
    public string Token { get; private set; } = default!;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsActive => !IsRevoked && !IsExpired;

    public static RefreshToken Create(string token, DateTime expiresAt, Guid userId)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = token,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };
    }

    public void Revoke()
    {
        if (!RevokedAt.HasValue)
        {
            RevokedAt = DateTime.UtcNow;
        }
    }
}

