namespace Simu.Core.Entities;

/// <summary>
/// Represents a transaction detail line item
/// </summary>
public class DetalleTransaccion
{
    public int IdDetalleTransaccion { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int Cantidad { get; set; }
    public decimal Subtotal { get; set; }
    
    // Foreign Keys
    public int IdTransaccion { get; set; }
    public int IdProducto { get; set; }
    
    // Navigation Properties
    public virtual Transaccion? Transaccion { get; set; }
    public virtual Producto? Producto { get; set; }
}
