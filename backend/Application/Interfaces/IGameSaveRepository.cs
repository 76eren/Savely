using Domain;

namespace Application.Interfaces;

public interface IGameSaveRepository
{
    Task<GameSave?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GameSave?> GetByIdForProfileAsync(Guid id, Guid profileId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GameSave>> GetByProfileIdAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<GameSave?> GetLatestByProfileIdAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<GameSave?> GetByProfileIdAndNameAsync(Guid profileId, string name, CancellationToken cancellationToken = default);
    Task AddAsync(GameSave save, CancellationToken cancellationToken = default);
    void Remove(GameSave save);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
