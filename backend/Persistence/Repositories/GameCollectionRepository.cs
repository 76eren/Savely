using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class GameCollectionRepository(AppDbContext dbContext) : IGameCollectionRepository
{
    public Task<GameCollection?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.GameCollections.FirstOrDefaultAsync(collection => collection.Id == id, cancellationToken);
    }

    public Task<GameCollection?> GetByIdForUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.GameCollections.FirstOrDefaultAsync(
            collection => collection.Id == id && collection.UserId == userId,
            cancellationToken);
    }

    public Task<GameCollection?> GetByIdWithProfilesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.GameCollections
            .Include(collection => collection.Profiles)
            .FirstOrDefaultAsync(collection => collection.Id == id, cancellationToken);
    }

    public Task<GameCollection?> GetByIdWithProfilesAndSavesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.GameCollections
            .Include(collection => collection.Profiles)
            .ThenInclude(profile => profile.GameSaves)
            .FirstOrDefaultAsync(collection => collection.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<GameCollection>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.GameCollections
            .Where(collection => collection.UserId == userId)
            .OrderBy(collection => collection.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<GameCollection?> GetByUserIdAndNameAsync(
        Guid userId,
        string name,
        CancellationToken cancellationToken = default)
    {
        return dbContext.GameCollections.FirstOrDefaultAsync(
            collection => collection.UserId == userId && collection.Name == name,
            cancellationToken);
    }

    public async Task AddAsync(GameCollection collection, CancellationToken cancellationToken = default)
    {
        await dbContext.GameCollections.AddAsync(collection, cancellationToken);
    }

    public void Remove(GameCollection collection)
    {
        dbContext.GameCollections.Remove(collection);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
