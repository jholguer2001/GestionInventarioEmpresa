using MyApp.Business.Dtos.Item;
using MyApp.Entities.Enums;

namespace MyApp.Presentation.Models;

/// <summary>
/// ViewModel para la vista de índice de artículos con paginación.
/// Cumple con RF2.4: Listar y paginar artículos.
/// </summary>
public class ItemsIndexViewModel
{
    /// <summary>
    /// Resultado paginado de artículos usando nuestro tipo personalizado.
    /// </summary>
    public MyApp.Business.Dtos.Common.PagedResult<ItemDto> PagedItems { get; set; } = new MyApp.Business.Dtos.Common.PagedResult<ItemDto>();

    /// <summary>
    /// Término de búsqueda actual.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Categoría seleccionada para filtrar.
    /// </summary>
    public string? SelectedCategory { get; set; }

    /// <summary>
    /// Estado seleccionado para filtrar.
    /// </summary>
    public ItemStatus? SelectedStatus { get; set; }

    /// <summary>
    /// Lista de todas las categorías disponibles para el filtro.
    /// </summary>
    public List<string> Categories { get; set; } = new List<string>();

    /// <summary>
    /// Campo por el cual se está ordenando actualmente.
    /// </summary>
    public string SortBy { get; set; } = "Name";

    /// <summary>
    /// Dirección del ordenamiento actual.
    /// </summary>
    public string SortOrder { get; set; } = "asc";

    /// <summary>
    /// Tamaño de página actual.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Verifica si hay filtros activos.
    /// </summary>
    public bool HasActiveFilters =>
        !string.IsNullOrEmpty(SearchTerm) ||
        !string.IsNullOrEmpty(SelectedCategory) ||
        SelectedStatus.HasValue;
}