using Simu.Core.Entities;

namespace Simu.Business.Services;

/// <summary>
/// Service interface for Producto operations
/// </summary>
public interface IProductoService : IBaseService<Producto>
{
    Task<Producto?> GetByReferenciaAsync(string referencia);
    Task<List<Producto>> GetByTipoProductoAsync(int idTipoProducto);
    Task<List<Producto>> GetBajosStockAsync(int umbral = 10);
    Task<bool> ActualizarStockAsync(int idProducto, int cantidad);
}
