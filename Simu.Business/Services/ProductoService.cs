using Simu.Core.Entities;
using Simu.Data.Repositories;

namespace Simu.Business.Services;

/// <summary>
/// Implementation of Producto service
/// </summary>
public class ProductoService : BaseService<Producto>, IProductoService
{
    private readonly ProductoRepository _productoRepository;

    public ProductoService(ProductoRepository productoRepository) : base(productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<Producto?> GetByReferenciaAsync(string referencia)
    {
        if (string.IsNullOrWhiteSpace(referencia))
            throw new ArgumentException("Referencia no puede estar vacía", nameof(referencia));

        return await _productoRepository.GetByReferenciaAsync(referencia);
    }

    public async Task<List<Producto>> GetByTipoProductoAsync(int idTipoProducto)
    {
        if (idTipoProducto <= 0)
            throw new ArgumentException("IdTipoProducto debe ser mayor a 0", nameof(idTipoProducto));

        return await _productoRepository.GetByTipoProductoAsync(idTipoProducto);
    }

    public async Task<List<Producto>> GetBajosStockAsync(int umbral = 10)
    {
        if (umbral < 0)
            throw new ArgumentException("Umbral no puede ser negativo", nameof(umbral));

        return await _productoRepository.GetBajosStockAsync(umbral);
    }

    public async Task<bool> ActualizarStockAsync(int idProducto, int cantidad)
    {
        var producto = await _productoRepository.GetByIdAsync(idProducto);
        if (producto == null)
            throw new InvalidOperationException($"Producto con id {idProducto} no encontrado");

        if (producto.Stock + cantidad < 0)
            throw new InvalidOperationException("Stock no puede ser negativo");

        producto.Stock += cantidad;
        _productoRepository.Update(producto);
        return await _productoRepository.SaveChangesAsync();
    }
}
