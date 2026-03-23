namespace Simu.Core.Entities;

/// <summary>
/// Lookup table for departments (geographic regions)
/// </summary>
public class Departamento
{
    public int IdDepartamento { get; set; }
    public string Nombre { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual ICollection<Ciudad> Ciudades { get; set; } = new List<Ciudad>();
}
