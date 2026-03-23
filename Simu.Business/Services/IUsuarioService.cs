using Simu.Core.Entities;

namespace Simu.Business.Services;

/// <summary>
/// Service interface for Usuario operations
/// </summary>
public interface IUsuarioService : IBaseService<Usuario>
{
    Task<Usuario?> AuthenticateAsync(string username, string password);
    Task<Usuario?> GetByUsernameAsync(string username);
    Task<Usuario?> GetByEmailAsync(string email);
    Task<List<Usuario>> GetByRolAsync(int idRol);
    Task<bool> UserExistsAsync(string username, string email);
}
