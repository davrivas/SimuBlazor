namespace Simu.Core.Entities;

/// <summary>
/// Represents a user in the system with role-based access control
/// </summary>
public class Usuario
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Usuario1 { get; set; } = string.Empty; // Username
    public string Contrasena { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Cedula { get; set; }
    public int? IdTipoDocumento { get; set; }
    public DateTime FechaRegistro { get; set; }
    public bool Activo { get; set; } = true;
    
    // Foreign Keys
    public int IdRol { get; set; }
    public int? IdCiudad { get; set; }
    
    // Navigation Properties
    public virtual Rol? Rol { get; set; }
    public virtual Ciudad? Ciudad { get; set; }
    public virtual TipoDocumento? TipoDocumento { get; set; }
    public virtual ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    public virtual ICollection<Reparacion> Reparaciones { get; set; } = new List<Reparacion>();
}
