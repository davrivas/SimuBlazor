using Simu.Core.Entities;
using Simu.Data.Context;

namespace Simu.Data.Repositories;

/// <summary>
/// Repository for Usuario entity
/// </summary>
public class UsuarioRepository : BaseRepository<Usuario>
{
    public UsuarioRepository(SimuDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> GetByUsernameAsync(string username)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Usuario1 == username);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<Usuario>> GetByRolAsync(int idRol)
    {
        return await _dbSet
            .Where(u => u.IdRol == idRol)
            .ToListAsync();
    }
}
