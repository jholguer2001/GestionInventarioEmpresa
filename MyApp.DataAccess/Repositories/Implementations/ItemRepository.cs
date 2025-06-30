namespace MyApp.DataAccess.Repositories.Implementations;

/// <summary>
/// Implementación del repositorio para la entidad <see cref="Item"/>.
/// </summary>
public class ItemRepository : Repository<Item>, IItemRepository
{
    /// <summary>
    /// Inicializa una nueva instancia del repositorio de Item.
    /// </summary>
    /// <param name="context">El contexto de la base de datos inyectado.</param>
    public ItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene un artículo por su ID, incluyendo su historial de préstamos y los usuarios asociados.
    /// </summary>
    public override async Task<Item?> GetByIdAsync(int id)
    {
        return await _context.Items
            .Include(i => i.Loans)
                .ThenInclude(l => l.User)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    /// <summary>
    /// Obtiene todos los artículos, ordenados por nombre.
    /// </summary>
    public override async Task<IEnumerable<Item>> GetAllAsync()
    {
        return await _context.Items
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Item?> GetByCodeAsync(string code)
    {
        return await _context.Items
            .Include(i => i.Loans)
                .ThenInclude(l => l.User)
            .FirstOrDefaultAsync(i => i.Code.ToLower() == code.ToLower());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Item>> GetByCategoryAsync(string category)
    {
        return await _context.Items
            .Where(i => i.Category.ToLower() == category.ToLower())
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Item>> GetByStatusAsync(ItemStatus status)
    {
        return await _context.Items
            .Where(i => i.Status == status)
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Item>> SearchAsync(string searchTerm)
    {
        return await _context.Items
            .Where(i => i.Name.Contains(searchTerm) ||
                        i.Code.Contains(searchTerm) ||
                        i.Category.Contains(searchTerm) ||
                        i.Location.Contains(searchTerm))
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> CodeExistsAsync(string code)
    {
        return await _context.Items
            .AnyAsync(i => i.Code.ToLower() == code.ToLower());
    }

    /// <inheritdoc/>
    public async Task<bool> CodeExistsForOtherItemAsync(string code, int itemId)
    {
        return await _context.Items
            .AnyAsync(i => i.Code.ToLower() == code.ToLower() && i.Id != itemId);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        return await _context.Items
            .Select(i => i.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Item>> GetAvailableItemsAsync()
    {
        return await _context.Items
            .Where(i => i.Status == ItemStatus.Available)
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Item>> GetFilteredAsync(string? searchTerm, string? category, ItemStatus? status)
    {
        var query = _context.Items.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(i => i.Name.Contains(searchTerm) ||
                                     i.Code.Contains(searchTerm) ||
                                     i.Category.Contains(searchTerm) ||
                                     i.Location.Contains(searchTerm));
        }

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(i => i.Category.ToLower() == category.ToLower());
        }

        if (status.HasValue)
        {
            query = query.Where(i => i.Status == status.Value);
        }

        return await query.OrderBy(i => i.Name).ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Item>> GetItemsPagedAsync(int pageNumber, int pageSize, string? category = null, ItemStatus? status = null)
    {
        var query = _context.Items.AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(i => i.Category.ToLower() == category.ToLower());
        }

        if (status.HasValue)
        {
            query = query.Where(i => i.Status == status.Value);
        }

        return await query
            .OrderBy(i => i.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}