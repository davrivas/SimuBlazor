using Simu.Core.Entities;

namespace Simu.Business.Services;

/// <summary>
/// Base service interface with common operations
/// </summary>
public interface IBaseService<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> SaveChangesAsync();
}
