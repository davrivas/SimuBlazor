namespace Simu.Core.Entities;

/// <summary>
/// Lookup table for document types
/// </summary>
public class TipoDocumento
{
    public int IdTipoDocumento { get; set; }
    public string Nombre { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
