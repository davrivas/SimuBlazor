namespace Simu.Core.Entities;

/// <summary>
/// Represents a role in the system (Admin, Mechanic, Sales, etc.)
/// </summary>
public class Rol
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    
    // Navigation Properties
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
}
