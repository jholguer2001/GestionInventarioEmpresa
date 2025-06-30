namespace MyApp.DataAccess.Repositories.Implementations;

/// <summary>
/// Implementación del repositorio para la entidad <see cref="User"/>.
/// </summary>
public class UserRepository : Repository<User>, IUserRepository
{
    /// <summary>
    /// Inicializa una nueva instancia del repositorio de User.
    /// </summary>
    /// <param name="context">El contexto de la base de datos inyectado.</param>
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene un usuario por su ID, incluyendo su Rol y su historial de Préstamos con sus Artículos.
    /// </summary>
    public override async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Loans)
                .ThenInclude(l => l.Item)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <summary>
    /// Obtiene todos los usuarios, incluyendo su Rol, ordenados por nombre.
    /// </summary>
    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.Role)
            .OrderBy(u => u.Name)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<User>> GetByRoleAsync(int roleId)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Where(u => u.RoleId == roleId)
            .OrderBy(u => u.Name)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Role)
            .Where(u => u.IsActive)
            .OrderBy(u => u.Name)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> EmailExistsForOtherUserAsync(string email, int userId)
    {
        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower() && u.Id != userId);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Name.Contains(searchTerm) || u.Email.Contains(searchTerm))
            .OrderBy(u => u.Name)
            .ToListAsync();
    }
}
