namespace Simu.Core.Entities;

/// <summary>
/// Lookup table for cities (geographic locations)
/// </summary>
public class Ciudad
{
    public int IdCiudad { get; set; }
    public string Nombre { get; set; } = string.Empty;
    
    // Foreign Keys
    public int IdDepartamento { get; set; }
    
    // Navigation Properties
    public virtual Departamento? Departamento { get; set; }
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
