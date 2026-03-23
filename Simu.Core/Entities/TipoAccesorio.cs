namespace Simu.Core.Entities;

/// <summary>
/// Lookup table for accessory types
/// </summary>
public class TipoAccesorio
{
    public int IdTipoAccesorio { get; set; }
    public string Nombre { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual ICollection<Accesorio> Accesorios { get; set; } = new List<Accesorio>();
}
