namespace Domain;

public class User
{
    public Guid Id { get; private set; } = default!;
    public string UserHandle { get; private set; } = default!; // This is the @username, and must be unique
    public string UserName { get; private set; } = default!;
    public string Bio { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    
    // TODO: Add profile picture later when min/io has been implemented

    public static User Create(
        string userHandle,
        string userName,
        string bio,
        string email,
        string passwordHash
    )
    {
        return new User
        {
            Id = Guid.NewGuid(),
            UserHandle = userHandle,
            UserName = userName,
            Bio = bio,
            Email = email,
            PasswordHash = passwordHash
        };
    }
    
}