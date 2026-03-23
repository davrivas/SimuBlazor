using System.Security.Cryptography;
using System.Text;
using Simu.Core.Entities;
using Simu.Data.Repositories;

namespace Simu.Business.Services;

/// <summary>
/// Implementation of Usuario service
/// </summary>
public class UsuarioService : BaseService<Usuario>, IUsuarioService
{
    private readonly UsuarioRepository _usuarioRepository;

    public UsuarioService(UsuarioRepository usuarioRepository) : base(usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Usuario?> AuthenticateAsync(string username, string password)
    {
        var usuario = await _usuarioRepository.GetByUsernameAsync(username);
        if (usuario == null || !VerifyPassword(password, usuario.Contrasena))
        {
            return null;
        }

        return usuario;
    }

    public async Task<Usuario?> GetByUsernameAsync(string username)
    {
        return await _usuarioRepository.GetByUsernameAsync(username);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _usuarioRepository.GetByEmailAsync(email);
    }

    public async Task<List<Usuario>> GetByRolAsync(int idRol)
    {
        return await _usuarioRepository.GetByRolAsync(idRol);
    }

    public async Task<bool> UserExistsAsync(string username, string email)
    {
        var userByUsername = await _usuarioRepository.GetByUsernameAsync(username);
        var userByEmail = await _usuarioRepository.GetByEmailAsync(email);
        return userByUsername != null || userByEmail != null;
    }

    /// <summary>
    /// Hash password using SHA256
    /// </summary>
    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    /// <summary>
    /// Verify password against hash
    /// </summary>
    private static bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hash;
    }
}
