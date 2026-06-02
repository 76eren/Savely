using Domain;

namespace Application.Interfaces;

public interface IGameProfileRepository
{
    Task<GameProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GameProfile?> GetByIdForCollectionAsync(
        Guid id,
        Guid collectionId,
        CancellationToken cancellationToken = default);
    Task<GameProfile?> GetByIdWithSavesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GameProfile>> GetByCollectionIdAsync(Guid collectionId, CancellationToken cancellationToken = default);
    Task<GameProfile?> GetByCollectionIdAndNameAsync(Guid collectionId, string name, CancellationToken cancellationToken = default);
    Task AddAsync(GameProfile profile, CancellationToken cancellationToken = default);
    void Remove(GameProfile profile);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
