using System.ComponentModel.DataAnnotations;

namespace MyApp.Business.Dtos.Item;

/// <summary>
/// DTO que contiene los datos necesarios para crear un nuevo artículo.
/// </summary>
public class CreateItemDto
{
    /// <summary>
    /// El código único del artículo.
    /// </summary>
    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(20, ErrorMessage = "El código no puede exceder los 20 caracteres.")]
    public string Code { get; set; }

    /// <summary>
    /// El nombre descriptivo del artículo.
    /// </summary>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string Name { get; set; }

    /// <summary>
    /// La categoría a la que pertenece el artículo.
    /// </summary>
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [StringLength(50, ErrorMessage = "La categoría no puede exceder los 50 caracteres.")]
    public string Category { get; set; }

    /// <summary>
    /// El estado inicial del artículo (ej. Disponible).
    /// </summary>
    public ItemStatus Status { get; set; } = ItemStatus.Available;

    /// <summary>
    /// La ubicación física o de almacenamiento del artículo.
    /// </summary>
    [StringLength(100, ErrorMessage = "La ubicación no puede exceder los 100 caracteres.")]
    public string? Location { get; set; }
}
