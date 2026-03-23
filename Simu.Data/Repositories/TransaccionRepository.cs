using Simu.Core.Entities;
using Simu.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Simu.Data.Repositories;

/// <summary>
/// Repository for Transaccion entity
/// </summary>
public class TransaccionRepository : BaseRepository<Transaccion>
{
    public TransaccionRepository(SimuDbContext context) : base(context)
    {
    }

    public async Task<List<Transaccion>> GetByFechaRangeAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        return await _dbSet
            .Where(t => t.Fecha >= fechaInicio && t.Fecha <= fechaFin)
            .Include(t => t.Moto)
            .Include(t => t.Usuario)
            .Include(t => t.TipoTransaccion)
            .Include(t => t.Detalles)
            .ToListAsync();
    }

    public async Task<List<Transaccion>> GetByMotoAsync(int idMoto)
    {
        return await _dbSet
            .Where(t => t.IdMoto == idMoto)
            .Include(t => t.Detalles)
            .OrderByDescending(t => t.Fecha)
            .ToListAsync();
    }

    public async Task<List<Transaccion>> GetByUsuarioAsync(int idUsuario)
    {
        return await _dbSet
            .Where(t => t.IdUsuario == idUsuario)
            .Include(t => t.Moto)
            .Include(t => t.Detalles)
            .OrderByDescending(t => t.Fecha)
            .ToListAsync();
    }
}
