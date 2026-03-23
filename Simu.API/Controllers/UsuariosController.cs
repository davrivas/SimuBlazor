using Microsoft.AspNetCore.Mvc;
using Simu.Core.Entities;
using Simu.Business.Services;

namespace Simu.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger)
    {
        _usuarioService = usuarioService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<Usuario>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var usuario = await _usuarioService.AuthenticateAsync(request.Username, request.Password);
            if (usuario == null)
                return Unauthorized("Usuario o contraseña inválidos");

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetById(int id)
    {
        try
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting usuario");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("username/{username}")]
    public async Task<ActionResult<Usuario>> GetByUsername(string username)
    {
        try
        {
            var usuario = await _usuarioService.GetByUsernameAsync(username);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting usuario by username");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> Create(Usuario usuario)
    {
        try
        {
            var userExists = await _usuarioService.UserExistsAsync(usuario.Usuario1, usuario.Email);
            if (userExists)
                return BadRequest("El usuario o email ya existe");

            usuario.Contrasena = UsuarioService.HashPassword(usuario.Contrasena);
            usuario.FechaRegistro = DateTime.Now;

            await _usuarioService.AddAsync(usuario);
            await _usuarioService.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = usuario.IdUsuario }, usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating usuario");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
