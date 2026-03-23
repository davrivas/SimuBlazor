namespace Simu.Core.Entities;

/// <summary>
/// Represents a motorcycle in the system
/// </summary>
public class Moto
{
    public int IdMoto { get; set; }
    public string Placa { get; set; } = string.Empty;
    public int Cilindraje { get; set; }
    public string? Color { get; set; }
    public int? Modelo { get; set; }
    public int? Kilometraje { get; set; }
    public DateTime FechaIngreso { get; set; }
    public bool Activo { get; set; } = true;
    
    // Foreign Keys
    public int IdMarca { get; set; }
    public int IdEstadoMoto { get; set; }
    
    // Navigation Properties
    public virtual Marca? Marca { get; set; }
    public virtual EstadoMoto? EstadoMoto { get; set; }
    public virtual ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    public virtual ICollection<Reparacion> Reparaciones { get; set; } = new List<Reparacion>();
}
