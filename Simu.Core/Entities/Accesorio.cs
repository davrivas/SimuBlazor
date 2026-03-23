namespace Simu.Core.Entities;

/// <summary>
/// Represents an accessory (motorcycle parts)
/// </summary>
public class Accesorio
{
    public int IdAccesorio { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public bool Activo { get; set; } = true;
    
    // Foreign Keys
    public int IdTipoAccesorio { get; set; }
    
    // Navigation Properties
    public virtual TipoAccesorio? TipoAccesorio { get; set; }
}
