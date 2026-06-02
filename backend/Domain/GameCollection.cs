namespace Domain;

public class GameCollection
{
    public Guid Id { get; set; } = default!;
    public Guid UserId { get; set; } = default!;
    public User User { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<GameProfile> Profiles { get; set; } = new List<GameProfile>();

    public static GameCollection create(User user, string name)
    {
        var now = DateTime.UtcNow;

        return new GameCollection
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            User = user,
            Name = name,
            CreatedAt = now,
            UpdatedAt = now
        };

    }
}
