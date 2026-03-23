using Simu.Core.Entities;

namespace Simu.Business.Services;

/// <summary>
/// Service interface for Transaccion operations
/// </summary>
public interface ITransaccionService : IBaseService<Transaccion>
{
    Task<List<Transaccion>> GetByFechaRangeAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<List<Transaccion>> GetByMotoAsync(int idMoto);
    Task<List<Transaccion>> GetByUsuarioAsync(int idUsuario);
    Task<decimal> CalcularTotalAsync(int idTransaccion);
}
