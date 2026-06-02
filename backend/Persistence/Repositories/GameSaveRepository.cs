using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class GameSaveRepository : IGameSaveRepository
{
    private readonly AppDbContext _dbContext;

    public GameSaveRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<GameSave?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.GameSaves.FirstOrDefaultAsync(save => save.Id == id, cancellationToken);
    }

    public Task<GameSave?> GetByIdForProfileAsync(
        Guid id,
        Guid profileId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.GameSaves.FirstOrDefaultAsync(
            save => save.Id == id && save.GameProfileId == profileId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<GameSave>> GetByProfileIdAsync(
        Guid profileId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.GameSaves
            .Where(save => save.GameProfileId == profileId)
            .OrderByDescending(save => save.UpdatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<GameSave?> GetLatestByProfileIdAsync(Guid profileId, CancellationToken cancellationToken = default)
    {
        return _dbContext.GameSaves
            .Where(save => save.GameProfileId == profileId)
            .OrderByDescending(save => save.UpdatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<GameSave?> GetByProfileIdAndNameAsync(
        Guid profileId,
        string name,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.GameSaves.FirstOrDefaultAsync(
            save => save.GameProfileId == profileId && save.Name == name,
            cancellationToken);
    }

    public async Task AddAsync(GameSave save, CancellationToken cancellationToken = default)
    {
        await _dbContext.GameSaves.AddAsync(save, cancellationToken);
    }

    public void Remove(GameSave save)
    {
        _dbContext.GameSaves.Remove(save);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
