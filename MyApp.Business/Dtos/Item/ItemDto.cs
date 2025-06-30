namespace MyApp.Business.Dtos.Item;

/// <summary>
/// DTO que representa los datos de un artículo para ser mostrados al cliente.
/// </summary>
public class ItemDto
{
    /// <summary>
    /// El identificador único del artículo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El código único del artículo.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// El nombre descriptivo del artículo.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// La categoría a la que pertenece el artículo.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// El estado actual del artículo (ej. Disponible, Prestado).
    /// </summary>
    public ItemStatus Status { get; set; }

    /// <summary>
    /// La ubicación física o de almacenamiento del artículo.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// La fecha y hora en que el artículo fue creado.
    /// </summary>
    public DateTime CreatedDate { get; set; }
}