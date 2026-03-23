using Microsoft.EntityFrameworkCore;
using Simu.Data.Context;

namespace Simu.Data.Repositories;

/// <summary>
/// Base repository with common CRUD operations
/// </summary>
public abstract class BaseRepository<T> where T : class
{
    protected readonly SimuDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(SimuDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
