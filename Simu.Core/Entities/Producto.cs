namespace Simu.Core.Entities;

/// <summary>
/// Represents a product (accessory or part)
/// </summary>
public class Producto
{
    public int IdProducto { get; set; }
    public string Referencia { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public decimal? PorcentajeDescuento { get; set; }
    public int Stock { get; set; }
    public string? FotoUrl { get; set; }
    public string? AltFoto { get; set; }
    public bool Activo { get; set; } = true;
    
    // Foreign Keys
    public int IdTipoProducto { get; set; }
    
    // Navigation Properties
    public virtual TipoProducto? TipoProducto { get; set; }
    public virtual ICollection<DetalleTransaccion> DetallesTransacciones { get; set; } = new List<DetalleTransaccion>();
}
