using Simu.Core.Entities;
using Simu.Data.Context;
using Simu.Data.Repositories;

namespace Simu.Business.Services;

/// <summary>
/// Implementation of base service with common CRUD operations
/// </summary>
public abstract class BaseService<T> : IBaseService<T> where T : class
{
    protected readonly BaseRepository<T> _repository;

    protected BaseService(BaseRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _repository.AddAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _repository.Update(entity);
        await SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _repository.Delete(entity);
        await SaveChangesAsync();
    }

    public virtual async Task<bool> SaveChangesAsync()
    {
        return await _repository.SaveChangesAsync();
    }
}
