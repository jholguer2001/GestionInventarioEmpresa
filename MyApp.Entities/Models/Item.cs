namespace MyApp.Entities.Models;
/// <summary>
/// Representa un artículo individual en el inventario.
/// </summary>
public class Item : IAuditableEntity
{
    /// <summary>
    /// Identificador único del artículo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Código único asignado al artículo para una rápida identificación.
    /// </summary>
    [Required]
    [StringLength(20)]
    public string Code { get; set; }

    /// <summary>
    /// Nombre descriptivo del artículo.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Categoría a la que pertenece el artículo (ej. "Electrónica", "Libros").
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Category { get; set; }

    /// <summary>
    /// Estado actual del artículo (ej. Disponible, Prestado, En Mantenimiento).
    /// El valor por defecto es "Disponible".
    /// </summary>
    public ItemStatus Status { get; set; } = ItemStatus.Available;

    /// <summary>
    /// Ubicación física o de almacenamiento del artículo.
    /// </summary>
    [StringLength(100)]
    public string Location { get; set; }

    // --- Campos de Auditoría (Implementación de IAuditableEntity) ---

    /// <summary>
    /// La fecha y hora en que el artículo fue registrado por primera vez.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// La fecha y hora de la última modificación del artículo.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// El usuario que registró el artículo.
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// El último usuario que modificó el artículo.
    /// </summary>
    public string? ModifiedBy { get; set; }

    // --- Propiedades de Navegación ---

    /// <summary>
    /// Colección de préstamos asociados a este artículo.
    /// </summary>
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
