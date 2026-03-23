using Simu.Core.Entities;

namespace Simu.Business.Services;

/// <summary>
/// Service interface for Moto operations
/// </summary>
public interface IMotoService : IBaseService<Moto>
{
    Task<Moto?> GetByPlacaAsync(string placa);
    Task<List<Moto>> GetByMarcaAsync(int idMarca);
    Task<List<Moto>> GetActivosAsync();
}
