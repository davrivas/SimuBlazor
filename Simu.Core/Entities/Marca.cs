namespace Simu.Core.Entities;

/// <summary>
/// Lookup table for motorcycle brands
/// </summary>
public class Marca
{
    public int IdMarca { get; set; }
    public string Nombre { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual ICollection<Moto> Motos { get; set; } = new List<Moto>();
}
