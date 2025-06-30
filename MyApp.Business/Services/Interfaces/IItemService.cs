

using MyApp.Business.Dtos.Common;

namespace MyApp.Business.Services.Interfaces;

/// <summary>
/// Define un contrato para el servicio de negocio que maneja la lógica de los artículos.
/// Actualizado para soportar paginación y ordenamiento (RF2.4).
/// </summary>
public interface IItemService
{
    /// <summary>
    /// Obtiene todos los artículos.
    /// </summary>
    /// <returns>Una colección de DTOs de artículos.</returns>
    Task<IEnumerable<ItemDto>> GetAllItemsAsync();

    /// <summary>
    /// RF2.4: Obtiene artículos paginados con filtros y ordenamiento.
    /// </summary>
    /// <param name="parameters">Parámetros de filtrado, paginación y ordenamiento.</param>
    /// <returns>Resultado paginado de artículos.</returns>
    Task<PagedResult<ItemDto>> GetItemsPagedAsync(ItemFilterParameters parameters);

    /// <summary>
    /// Obtiene un artículo específico por su ID.
    /// </summary>
    /// <param name="id">El ID del artículo a buscar.</param>
    /// <returns>Un DTO del artículo encontrado, o nulo si no existe.</returns>
    Task<ItemDto?> GetItemByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo artículo en el sistema.
    /// </summary>
    /// <param name="createItemDto">El DTO con los datos para crear el artículo.</param>
    /// <returns>Un DTO del artículo recién creado.</returns>
    Task<ItemDto> CreateItemAsync(CreateItemDto createItemDto);

    /// <summary>
    /// Actualiza un artículo existente.
    /// </summary>
    /// <param name="id">El ID del artículo a actualizar.</param>
    /// <param name="updateItemDto">El DTO con los nuevos datos del artículo.</param>
    /// <returns>Un DTO del artículo actualizado.</returns>
    Task<ItemDto> UpdateItemAsync(int id, UpdateItemDto updateItemDto);

    /// <summary>
    /// Elimina un artículo del sistema.
    /// </summary>
    /// <param name="id">El ID del artículo a eliminar.</param>
    /// <returns>True si la eliminación fue exitosa; de lo contrario, false.</returns>
    Task<bool> DeleteItemAsync(int id);

    /// <summary>
    /// Busca artículos por un término de búsqueda.
    /// </summary>
    /// <param name="searchTerm">El término a buscar en el nombre, código, etc.</param>
    /// <returns>Una colección de DTOs de artículos que coinciden con la búsqueda.</returns>
    Task<IEnumerable<ItemDto>> SearchItemsAsync(string searchTerm);

    /// <summary>
    /// Obtiene artículos filtrados por una categoría específica.
    /// </summary>
    /// <param name="category">La categoría para filtrar.</param>
    /// <returns>Una colección de DTOs de artículos.</returns>
    Task<IEnumerable<ItemDto>> GetItemsByCategoryAsync(string category);

    /// <summary>
    /// Obtiene artículos filtrados por un estado específico.
    /// </summary>
    /// <param name="status">El estado para filtrar.</param>
    /// <returns>Una colección de DTOs de artículos.</returns>
    Task<IEnumerable<ItemDto>> GetItemsByStatusAsync(ItemStatus status);

    /// <summary>
    /// Verifica si un artículo está disponible para ser prestado.
    /// </summary>
    /// <param name="itemId">El ID del artículo a verificar.</param>
    /// <returns>True si el artículo está disponible; de lo contrario, false.</returns>
    Task<bool> IsItemAvailableForLoanAsync(int itemId);

    /// <summary>
    /// Obtiene artículos aplicando múltiples filtros combinados.
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda opcional.</param>
    /// <param name="category">Categoría opcional.</param>
    /// <param name="status">Estado opcional.</param>
    /// <returns>Una colección de DTOs de artículos que cumplen con los filtros.</returns>
    Task<IEnumerable<ItemDto>> GetFilteredItemsAsync(string? searchTerm, string? category, ItemStatus? status);

    /// <summary>
    /// Obtiene todas las categorías disponibles.
    /// </summary>
    /// <returns>Lista de categorías únicas.</returns>
    Task<IEnumerable<string>> GetCategoriesAsync();
}