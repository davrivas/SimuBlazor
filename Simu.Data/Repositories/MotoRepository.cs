using Simu.Core.Entities;
using Simu.Data.Context;

namespace Simu.Data.Repositories;

/// <summary>
/// Repository for Moto entity
/// </summary>
public class MotoRepository : BaseRepository<Moto>
{
    public MotoRepository(SimuDbContext context) : base(context)
    {
    }

    public async Task<Moto?> GetByPlacaAsync(string placa)
    {
        return await _dbSet
            .Include(m => m.Marca)
            .Include(m => m.EstadoMoto)
            .FirstOrDefaultAsync(m => m.Placa == placa);
    }

    public async Task<List<Moto>> GetByMarcaAsync(int idMarca)
    {
        return await _dbSet
            .Where(m => m.IdMarca == idMarca)
            .Include(m => m.Marca)
            .Include(m => m.EstadoMoto)
            .ToListAsync();
    }

    public async Task<List<Moto>> GetActivosAsync()
    {
        return await _dbSet
            .Where(m => m.Activo)
            .Include(m => m.Marca)
            .Include(m => m.EstadoMoto)
            .ToListAsync();
    }
}
