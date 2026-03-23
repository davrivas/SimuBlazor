using Simu.Core.Entities;

namespace Simu.Business.Services;

/// <summary>
/// Service interface for Reparacion operations
/// </summary>
public interface IReparacionService : IBaseService<Reparacion>
{
    Task<List<Reparacion>> GetPendientesAsync();
    Task<List<Reparacion>> GetByMotoAsync(int idMoto);
    Task<bool> CerrarReparacionAsync(int idReparacion, decimal costo);
}
