using Microsoft.AspNetCore.Mvc;
using Simu.Core.Entities;
using Simu.Business.Services;

namespace Simu.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MotosController : ControllerBase
{
    private readonly IMotoService _motoService;
    private readonly ILogger<MotosController> _logger;

    public MotosController(IMotoService motoService, ILogger<MotosController> logger)
    {
        _motoService = motoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Moto>>> GetAll()
    {
        try
        {
            var motos = await _motoService.GetAllAsync();
            return Ok(motos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all motos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Moto>> GetById(int id)
    {
        try
        {
            var moto = await _motoService.GetByIdAsync(id);
            if (moto == null)
                return NotFound();

            return Ok(moto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting moto by id");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("placa/{placa}")]
    public async Task<ActionResult<Moto>> GetByPlaca(string placa)
    {
        try
        {
            var moto = await _motoService.GetByPlacaAsync(placa);
            if (moto == null)
                return NotFound();

            return Ok(moto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting moto by placa");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("activos")]
    public async Task<ActionResult<List<Moto>>> GetActivos()
    {
        try
        {
            var motos = await _motoService.GetActivosAsync();
            return Ok(motos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active motos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Moto>> Create(Moto moto)
    {
        try
        {
            await _motoService.AddAsync(moto);
            await _motoService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = moto.IdMoto }, moto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating moto");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Moto moto)
    {
        if (id != moto.IdMoto)
            return BadRequest();

        try
        {
            await _motoService.UpdateAsync(moto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating moto");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var moto = await _motoService.GetByIdAsync(id);
            if (moto == null)
                return NotFound();

            await _motoService.DeleteAsync(moto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting moto");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
