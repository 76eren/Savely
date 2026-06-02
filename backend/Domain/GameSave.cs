namespace Domain;

public class GameSave
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public long Size { get; set; }
    public Guid GameProfileId { get; set; } = default!;
    public GameProfile GameProfile { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static GameSave create(string name, long size, GameProfile gameProfile)
    {
        var now = DateTime.UtcNow;

        return new GameSave
        {
            Id = Guid.NewGuid(),
            Name = name,
            Size = size,
            GameProfileId = gameProfile.Id,
            GameProfile = gameProfile,
            CreatedAt = now,
            UpdatedAt = now
        };
    }
}
