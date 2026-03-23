using Simu.Core.Entities;
using Simu.Data.Context;

namespace Simu.Data.Repositories;

/// <summary>
/// Repository for Producto entity
/// </summary>
public class ProductoRepository : BaseRepository<Producto>
{
    public ProductoRepository(SimuDbContext context) : base(context)
    {
    }

    public async Task<Producto?> GetByReferenciaAsync(string referencia)
    {
        return await _dbSet
            .Include(p => p.TipoProducto)
            .FirstOrDefaultAsync(p => p.Referencia == referencia);
    }

    public async Task<List<Producto>> GetByTipoProductoAsync(int idTipoProducto)
    {
        return await _dbSet
            .Where(p => p.IdTipoProducto == idTipoProducto && p.Activo)
            .Include(p => p.TipoProducto)
            .ToListAsync();
    }

    public async Task<List<Producto>> GetBajosStockAsync(int umbral = 10)
    {
        return await _dbSet
            .Where(p => p.Stock <= umbral && p.Activo)
            .OrderBy(p => p.Stock)
            .ToListAsync();
    }
}
