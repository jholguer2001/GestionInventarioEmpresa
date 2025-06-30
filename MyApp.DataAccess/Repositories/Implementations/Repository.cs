namespace MyApp.DataAccess.Repositories.Implementations;

/// <summary>
/// Implementación genérica y base para todos los repositorios.
/// </summary>
/// <typeparam name="T">El tipo de la entidad.</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio genérico.
    /// </summary>
    /// <param name="context">El contexto de la base de datos inyectado.</param>
    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Obtiene una entidad por su ID. Puede ser sobrescrito para incluir datos relacionados.
    /// </summary>
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Obtiene todas las entidades. Puede ser sobrescrito para añadir ordenamiento o includes.
    /// </summary>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate);
    }

    /// <inheritdoc/>
    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    /// <inheritdoc/>
    public virtual async Task<bool> ExistsAsync(int id)
    {
        // FindAsync es eficiente para esto ya que solo busca por clave primaria.
        var entity = await _dbSet.FindAsync(id);
        return entity != null;
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _dbSet
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
