namespace MyApp.DataAccess.Repositories.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de artículos (Items),
/// extendiendo las operaciones CRUD básicas con consultas específicas.
/// </summary>
public interface IItemRepository : IRepository<Item>
{
    /// <summary>
    /// Obtiene un artículo por su código único.
    /// </summary>
    /// <param name="code">El código del artículo a buscar.</param>
    /// <returns>El artículo encontrado, o nulo si no existe.</returns>
    Task<Item?> GetByCodeAsync(string code);

    /// <summary>
    /// Obtiene todos los artículos que pertenecen a una categoría específica.
    /// </summary>
    /// <param name="category">El nombre de la categoría.</param>
    /// <returns>Una colección de artículos de la categoría especificada.</returns>
    Task<IEnumerable<Item>> GetByCategoryAsync(string category);

    /// <summary>
    /// Obtiene todos los artículos que tienen un estado específico.
    /// </summary>
    /// <param name="status">El estado del artículo a filtrar.</param>
    /// <returns>Una colección de artículos con el estado especificado.</returns>
    Task<IEnumerable<Item>> GetByStatusAsync(ItemStatus status);

    /// <summary>
    /// Busca artículos cuyo nombre o código contenga el término de búsqueda.
    /// </summary>
    /// <param name="searchTerm">El término a buscar.</param>
    /// <returns>Una colección de artículos que coinciden con la búsqueda.</returns>
    Task<IEnumerable<Item>> SearchAsync(string searchTerm);

    /// <summary>
    /// Verifica si ya existe un artículo con el código especificado.
    /// </summary>
    /// <param name="code">El código a verificar.</param>
    /// <returns>True si el código ya existe; de lo contrario, false.</returns>
    Task<bool> CodeExistsAsync(string code);

    /// <summary>
    /// Verifica si un código ya está en uso por otro artículo diferente.
    /// Útil al actualizar un artículo para evitar duplicar códigos.
    /// </summary>
    /// <param name="code">El código a verificar.</param>
    /// <param name="itemId">El ID del artículo que se está editando.</param>
    /// <returns>True si el código existe en otro artículo; de lo contrario, false.</returns>
    Task<bool> CodeExistsForOtherItemAsync(string code, int itemId);

    /// <summary>
    /// Obtiene una lista de todas las categorías de artículos distintas.
    /// </summary>
    /// <returns>Una colección de nombres de categorías.</returns>
    Task<IEnumerable<string>> GetCategoriesAsync();

    /// <summary>
    /// Obtiene todos los artículos que están disponibles para préstamo.
    /// </summary>
    /// <returns>Una colección de artículos con estado "Available".</returns>
    Task<IEnumerable<Item>> GetAvailableItemsAsync();

    /// <summary>
    /// Obtiene una página de artículos, opcionalmente filtrados por categoría y/o estado.
    /// </summary>
    /// <param name="pageNumber">El número de página a recuperar.</param>
    /// <param name="pageSize">El tamaño de la página.</param>
    /// <param name="category">La categoría opcional para filtrar.</param>
    /// <param name="status">El estado opcional para filtrar.</param>
    /// <returns>Una colección paginada de artículos.</returns>
    Task<IEnumerable<Item>> GetItemsPagedAsync(int pageNumber, int pageSize, string? category = null, ItemStatus? status = null);

    /// <summary>
    /// Obtiene artículos aplicando múltiples filtros combinados.
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda opcional para nombre o código.</param>
    /// <param name="category">Categoría opcional para filtrar.</param>
    /// <param name="status">Estado opcional para filtrar.</param>
    /// <returns>Una colección de artículos que cumplen con todos los filtros aplicados.</returns>
    Task<IEnumerable<Item>> GetFilteredAsync(string? searchTerm, string? category, ItemStatus? status);
}