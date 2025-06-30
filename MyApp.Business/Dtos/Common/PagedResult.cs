namespace MyApp.Business.Dtos.Common;

/// <summary>
/// Modelo genérico para resultados paginados.
/// Usado para cumplir con RF2.4: Listar y paginar artículos.
/// </summary>
/// <typeparam name="T">Tipo de datos a paginar.</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Lista de elementos de la página actual.
    /// </summary>
    public List<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Número de página actual (base 1).
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Total de páginas disponibles.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Cantidad de elementos por página.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total de elementos en toda la colección.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Indica si existe una página anterior.
    /// </summary>
    public bool HasPreviousPage => CurrentPage > 1;

    /// <summary>
    /// Indica si existe una página siguiente.
    /// </summary>
    public bool HasNextPage => CurrentPage < TotalPages;

    /// <summary>
    /// Número del primer elemento mostrado en la página actual.
    /// </summary>
    public int StartItem => TotalItems == 0 ? 0 : (CurrentPage - 1) * PageSize + 1;

    /// <summary>
    /// Número del último elemento mostrado en la página actual.
    /// </summary>
    public int EndItem => Math.Min(CurrentPage * PageSize, TotalItems);

    /// <summary>
    /// Constructor por defecto.
    /// </summary>
    public PagedResult()
    {
    }

    /// <summary>
    /// Constructor con parámetros.
    /// </summary>
    /// <param name="items">Lista de elementos.</param>
    /// <param name="totalItems">Total de elementos.</param>
    /// <param name="currentPage">Página actual.</param>
    /// <param name="pageSize">Tamaño de página.</param>
    public PagedResult(List<T> items, int totalItems, int currentPage, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
    }
}