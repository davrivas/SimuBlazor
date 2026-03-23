namespace Simu.Core.Entities;

/// <summary>
/// Represents a repair request
/// </summary>
public class Reparacion
{
    public int IdReparacion { get; set; }
    public DateTime FechaIngreso { get; set; }
    public DateTime? FechaSalida { get; set; }
    public decimal? Costo { get; set; }
    public string? Descripcion { get; set; }
    public string? Observaciones { get; set; }
    
    // Foreign Keys
    public int IdMoto { get; set; }
    public int IdUsuario { get; set; }
    public int IdTipoReparacion { get; set; }
    
    // Navigation Properties
    public virtual Moto? Moto { get; set; }
    public virtual Usuario? Usuario { get; set; }
    public virtual TipoReparacion? TipoReparacion { get; set; }
}
