namespace MyApp.DataAccess.Repositories.Implementations;

/// <summary>
/// Implementación del repositorio para la entidad <see cref="Role"/>.
/// </summary>
public class RoleRepository : Repository<Role>, IRoleRepository
{
    /// <summary>
    /// Inicializa una nueva instancia del repositorio de Role.
    /// </summary>
    /// <param name="context">El contexto de la base de datos inyectado.</param>
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene un rol por su ID, incluyendo la lista de usuarios asociados.
    /// </summary>
    public override async Task<Role?> GetByIdAsync(int id)
    {
        return await _context.Roles
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Obtiene todos los roles, incluyendo la lista de usuarios para cada rol,
    /// ordenados por nombre.
    /// </summary>
    public override async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _context.Roles
            .Include(r => r.Users)
            .OrderBy(r => r.Name)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Role>> GetActiveRolesAsync()
    {
        // Esta implementación devuelve roles que tienen al menos un usuario activo.
        return await _context.Roles
            .Where(r => r.Users.Any(u => u.IsActive))
            .OrderBy(r => r.Name)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> RoleExistsAsync(string name)
    {
        return await _context.Roles
            .AnyAsync(r => r.Name.ToLower() == name.ToLower());
    }

    /// <inheritdoc/>
    public async Task<bool> RoleHasUsersAsync(int roleId)
    {
        return await _context.Roles
            .Where(r => r.Id == roleId)
            .SelectMany(r => r.Users)
            .AnyAsync();
    }

    /// <inheritdoc/>
    public async Task<int> GetUserCountByRoleAsync(int roleId)
    {
        // Cuenta directamente en la tabla de usuarios para mayor eficiencia
        return await _context.Users
            .CountAsync(u => u.RoleId == roleId && u.IsActive);
    }
}