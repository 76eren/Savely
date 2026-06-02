using Domain;

namespace Application.Interfaces;

public interface IGameCollectionRepository
{
    Task<GameCollection?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GameCollection?> GetByIdForUserAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<GameCollection?> GetByIdWithProfilesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GameCollection?> GetByIdWithProfilesAndSavesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GameCollection>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<GameCollection?> GetByUserIdAndNameAsync(Guid userId, string name, CancellationToken cancellationToken = default);
    Task AddAsync(GameCollection collection, CancellationToken cancellationToken = default);
    void Remove(GameCollection collection);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
