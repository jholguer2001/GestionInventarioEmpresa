namespace MyApp.Entities.Models;

/// <summary>
/// Representa un rol de usuario en el sistema, utilizado para la autorización.
/// </summary>
public class Role : IAuditableEntity
{
    /// <summary>
    /// Identificador único del rol.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre único del rol (ej. "Administrator", "User").
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// Descripción opcional de las responsabilidades y permisos del rol.
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }

    // --- Campos de Auditoría (Implementación de IAuditableEntity) ---

    /// <summary>
    /// Fecha y hora en que se creó el rol.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Fecha y hora de la última modificación del rol.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// El usuario o proceso que creó el rol.
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// El último usuario o proceso que modificó el rol.
    /// </summary>
    public string? ModifiedBy { get; set; }

    // --- Propiedades de Navegación ---

    /// <summary>
    /// Colección de usuarios que están asignados a este rol.
    /// </summary>
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}