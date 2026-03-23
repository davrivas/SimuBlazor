namespace Simu.Core.Entities;

/// <summary>
/// Lookup table for repair types
/// </summary>
public class TipoReparacion
{
    public int IdTipoReparacion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual ICollection<Reparacion> Reparaciones { get; set; } = new List<Reparacion>();
}
