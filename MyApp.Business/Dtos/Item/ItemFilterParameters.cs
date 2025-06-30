namespace MyApp.Business.Dtos.Item;

/// <summary>
/// Parámetros para filtrado, paginación y ordenamiento de artículos.
/// Usado para cumplir con RF2.4: Listar y paginar artículos.
/// </summary>
public class ItemFilterParameters
{
    /// <summary>
    /// Número de página actual (base 1).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Cantidad de elementos por página.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Término de búsqueda para filtrar por nombre, código, categoría o ubicación.
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Categoría específica para filtrar.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Estado específico para filtrar.
    /// </summary>
    public ItemStatus? Status { get; set; }

    /// <summary>
    /// Campo por el cual ordenar: "Name", "Code", "Category", "CreatedDate".
    /// </summary>
    public string SortBy { get; set; } = "Name";

    /// <summary>
    /// Dirección del ordenamiento: "asc" o "desc".
    /// </summary>
    public string SortOrder { get; set; } = "asc";

    /// <summary>
    /// Valida que los parámetros estén en rangos correctos.
    /// </summary>
    public void Validate()
    {
        if (Page < 1) Page = 1;
        if (PageSize < 5) PageSize = 5;
        if (PageSize > 100) PageSize = 100;

        if (string.IsNullOrEmpty(SortBy))
            SortBy = "Name";

        if (SortOrder != "asc" && SortOrder != "desc")
            SortOrder = "asc";
    }
}