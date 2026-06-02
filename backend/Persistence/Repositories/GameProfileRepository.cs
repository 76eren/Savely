using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class GameProfileRepository : IGameProfileRepository
{
    private readonly AppDbContext _dbContext;

    public GameProfileRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<GameProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.GameProfiles.FirstOrDefaultAsync(profile => profile.Id == id, cancellationToken);
    }

    public Task<GameProfile?> GetByIdForCollectionAsync(
        Guid id,
        Guid collectionId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.GameProfiles.FirstOrDefaultAsync(
            profile => profile.Id == id && profile.GameCollectionId == collectionId,
            cancellationToken);
    }

    public Task<GameProfile?> GetByIdWithSavesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.GameProfiles
            .Include(profile => profile.GameSaves)
            .FirstOrDefaultAsync(profile => profile.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<GameProfile>> GetByCollectionIdAsync(
        Guid collectionId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.GameProfiles
            .Where(profile => profile.GameCollectionId == collectionId)
            .OrderBy(profile => profile.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<GameProfile?> GetByCollectionIdAndNameAsync(
        Guid collectionId,
        string name,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.GameProfiles.FirstOrDefaultAsync(
            profile => profile.GameCollectionId == collectionId && profile.Name == name,
            cancellationToken);
    }

    public async Task AddAsync(GameProfile profile, CancellationToken cancellationToken = default)
    {
        await _dbContext.GameProfiles.AddAsync(profile, cancellationToken);
    }

    public void Remove(GameProfile profile)
    {
        _dbContext.GameProfiles.Remove(profile);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
