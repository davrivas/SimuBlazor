namespace Simu.Core.Entities;

/// <summary>
/// Lookup table for motorcycle states (Active, In Repair, Sold, etc.)
/// </summary>
public class EstadoMoto
{
    public int IdEstadoMoto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual ICollection<Moto> Motos { get; set; } = new List<Moto>();
}
