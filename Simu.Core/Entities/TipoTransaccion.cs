namespace Simu.Core.Entities;

/// <summary>
/// Lookup table for transaction types (Sale, Purchase, etc.)
/// </summary>
public class TipoTransaccion
{
    public int IdTipoTransaccion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}
