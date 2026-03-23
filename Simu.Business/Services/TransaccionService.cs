using Simu.Core.Entities;
using Simu.Data.Repositories;

namespace Simu.Business.Services;

/// <summary>
/// Implementation of Transaccion service
/// </summary>
public class TransaccionService : BaseService<Transaccion>, ITransaccionService
{
    private readonly TransaccionRepository _transaccionRepository;

    public TransaccionService(TransaccionRepository transaccionRepository) : base(transaccionRepository)
    {
        _transaccionRepository = transaccionRepository;
    }

    public async Task<List<Transaccion>> GetByFechaRangeAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        if (fechaInicio > fechaFin)
            throw new ArgumentException("Fecha inicio no puede ser mayor que fecha fin");

        return await _transaccionRepository.GetByFechaRangeAsync(fechaInicio, fechaFin);
    }

    public async Task<List<Transaccion>> GetByMotoAsync(int idMoto)
    {
        if (idMoto <= 0)
            throw new ArgumentException("IdMoto debe ser mayor a 0", nameof(idMoto));

        return await _transaccionRepository.GetByMotoAsync(idMoto);
    }

    public async Task<List<Transaccion>> GetByUsuarioAsync(int idUsuario)
    {
        if (idUsuario <= 0)
            throw new ArgumentException("IdUsuario debe ser mayor a 0", nameof(idUsuario));

        return await _transaccionRepository.GetByUsuarioAsync(idUsuario);
    }

    public async Task<decimal> CalcularTotalAsync(int idTransaccion)
    {
        var transaccion = await _transaccionRepository.GetByIdAsync(idTransaccion);
        if (transaccion == null)
            throw new InvalidOperationException($"Transacción con id {idTransaccion} no encontrada");

        return transaccion.Total;
    }
}
