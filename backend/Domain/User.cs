using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User : IdentityUser<Guid>
{
    public string UserHandle { get; private set; } = default!; // This is the @username, and must be unique
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ICollection<GameCollection> Collections { get; private set; } = new List<GameCollection>();
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();

    // TODO: Add profile picture later when min/io has been implemented

    public static User Create(
        string userHandle,
        string userName,
        string email)
    {
        var now = DateTime.UtcNow;

        return new User
        {
            Id = Guid.NewGuid(),
            UserHandle = userHandle,
            UserName = userName,
            Email = email,
            CreatedAt = now,
            UpdatedAt = now,
            SecurityStamp = Guid.NewGuid().ToString()
        };
    }
}
