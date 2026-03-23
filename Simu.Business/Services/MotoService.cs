using Simu.Core.Entities;
using Simu.Data.Repositories;

namespace Simu.Business.Services;

/// <summary>
/// Implementation of Moto service
/// </summary>
public class MotoService : BaseService<Moto>, IMotoService
{
    private readonly MotoRepository _motoRepository;

    public MotoService(MotoRepository motoRepository) : base(motoRepository)
    {
        _motoRepository = motoRepository;
    }

    public async Task<Moto?> GetByPlacaAsync(string placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
            throw new ArgumentException("Placa no puede estar vacía", nameof(placa));

        return await _motoRepository.GetByPlacaAsync(placa);
    }

    public async Task<List<Moto>> GetByMarcaAsync(int idMarca)
    {
        if (idMarca <= 0)
            throw new ArgumentException("IdMarca debe ser mayor a 0", nameof(idMarca));

        return await _motoRepository.GetByMarcaAsync(idMarca);
    }

    public async Task<List<Moto>> GetActivosAsync()
    {
        return await _motoRepository.GetActivosAsync();
    }
}
