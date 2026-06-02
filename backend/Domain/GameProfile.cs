namespace Domain;

public class GameProfile
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid GameCollectionId { get; set; } = default!;
    public GameCollection GameCollection { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<GameSave> GameSaves { get; set; } = new List<GameSave>();

    public static GameProfile create(string name, GameCollection gameCollection)
    {
        var now = DateTime.UtcNow;

        return new GameProfile
        {
            Id = Guid.NewGuid(),
            Name = name,
            GameCollectionId = gameCollection.Id,
            GameCollection = gameCollection,
            CreatedAt = now,
            UpdatedAt = now
        };
    }
    
    // TODO: Add a list that stores the locations of save files later
}
