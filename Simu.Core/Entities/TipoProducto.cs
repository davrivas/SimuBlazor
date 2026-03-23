namespace Simu.Core.Entities;

/// <summary>
/// Lookup table for product types
/// </summary>
public class TipoProducto
{
    public int IdTipoProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
