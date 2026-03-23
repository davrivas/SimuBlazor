namespace Simu.Core.Entities;

/// <summary>
/// Represents a transaction (sale or purchase)
/// </summary>
public class Transaccion
{
    public int IdTransaccion { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
    
    // Foreign Keys
    public int IdMoto { get; set; }
    public int IdUsuario { get; set; }
    public int IdTipoTransaccion { get; set; }
    
    // Navigation Properties
    public virtual Moto? Moto { get; set; }
    public virtual Usuario? Usuario { get; set; }
    public virtual TipoTransaccion? TipoTransaccion { get; set; }
    public virtual ICollection<DetalleTransaccion> Detalles { get; set; } = new List<DetalleTransaccion>();
}
