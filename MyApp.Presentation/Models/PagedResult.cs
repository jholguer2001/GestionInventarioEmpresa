namespace MyApp.Presentation.Models;
/// <summary>
/// Modelo genérico para paginación
/// </summary>
/// <typeparam name="T">Tipo de datos a paginar</typeparam>
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    public int StartItem => (CurrentPage - 1) * PageSize + 1;
    public int EndItem => Math.Min(CurrentPage * PageSize, TotalItems);
}