using Simu.Core.Entities;
using Simu.Data.Repositories;

namespace Simu.Business.Services;

/// <summary>
/// Implementation of Reparacion service
/// </summary>
public class ReparacionService : BaseService<Reparacion>, IReparacionService
{
    private readonly BaseRepository<Reparacion> _reparacionRepository;

    public ReparacionService(BaseRepository<Reparacion> reparacionRepository) : base(reparacionRepository)
    {
        _reparacionRepository = reparacionRepository;
    }

    public async Task<List<Reparacion>> GetPendientesAsync()
    {
        var reparaciones = await _reparacionRepository.GetAllAsync();
        return reparaciones.Where(r => r.FechaSalida == null).ToList();
    }

    public async Task<List<Reparacion>> GetByMotoAsync(int idMoto)
    {
        if (idMoto <= 0)
            throw new ArgumentException("IdMoto debe ser mayor a 0", nameof(idMoto));

        var reparaciones = await _reparacionRepository.GetAllAsync();
        return reparaciones.Where(r => r.IdMoto == idMoto).ToList();
    }

    public async Task<bool> CerrarReparacionAsync(int idReparacion, decimal costo)
    {
        var reparacion = await _reparacionRepository.GetByIdAsync(idReparacion);
        if (reparacion == null)
            throw new InvalidOperationException($"Reparación con id {idReparacion} no encontrada");

        reparacion.FechaSalida = DateTime.Now;
        reparacion.Costo = costo;
        _reparacionRepository.Update(reparacion);
        return await _reparacionRepository.SaveChangesAsync();
    }
}
