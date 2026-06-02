namespace Domain;

public class User
{
    public Guid Id { get; private set; } = default!;
    public string UserHandle { get; private set; } = default!; // This is the @username, and must be unique
    public string UserName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ICollection<GameCollection> Collections { get; private set; } = new List<GameCollection>();
    
    // TODO: Add profile picture later when min/io has been implemented

    public static User Create(
        string userHandle,
        string userName,
        string email,
        string passwordHash
    )
    {
        var now = DateTime.UtcNow;

        return new User
        {
            Id = Guid.NewGuid(),
            UserHandle = userHandle,
            UserName = userName,
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = now,
            UpdatedAt = now
        };
    }
    
}
