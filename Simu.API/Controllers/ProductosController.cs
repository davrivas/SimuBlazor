using Microsoft.AspNetCore.Mvc;
using Simu.Core.Entities;
using Simu.Business.Services;

namespace Simu.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;
    private readonly ILogger<ProductosController> _logger;

    public ProductosController(IProductoService productoService, ILogger<ProductosController> logger)
    {
        _productoService = productoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Producto>>> GetAll()
    {
        try
        {
            var productos = await _productoService.GetAllAsync();
            return Ok(productos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all productos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Producto>> GetById(int id)
    {
        try
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting producto");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("referencia/{referencia}")]
    public async Task<ActionResult<Producto>> GetByReferencia(string referencia)
    {
        try
        {
            var producto = await _productoService.GetByReferenciaAsync(referencia);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting producto by referencia");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("bajos-stock")]
    public async Task<ActionResult<List<Producto>>> GetBajosStock([FromQuery] int umbral = 10)
    {
        try
        {
            var productos = await _productoService.GetBajosStockAsync(umbral);
            return Ok(productos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting productos with low stock");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Producto>> Create(Producto producto)
    {
        try
        {
            await _productoService.AddAsync(producto);
            await _productoService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = producto.IdProducto }, producto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating producto");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Producto producto)
    {
        if (id != producto.IdProducto)
            return BadRequest();

        try
        {
            await _productoService.UpdateAsync(producto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating producto");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
                return NotFound();

            await _productoService.DeleteAsync(producto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting producto");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost("{id}/actualizar-stock")]
    public async Task<IActionResult> ActualizarStock(int id, [FromBody] int cantidad)
    {
        try
        {
            var success = await _productoService.ActualizarStockAsync(id, cantidad);
            if (!success)
                return StatusCode(500, "Error al actualizar el stock");

            return Ok("Stock actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating stock");
            return StatusCode(500, ex.Message);
        }
    }
}
