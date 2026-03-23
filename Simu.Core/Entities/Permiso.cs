namespace Simu.Core.Entities;

/// <summary>
/// Represents a permission in the system
/// </summary>
public class Permiso
{
    public int IdPermiso { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    
    // Foreign Keys
    public int IdRol { get; set; }
    
    // Navigation Properties
    public virtual Rol? Rol { get; set; }
}
