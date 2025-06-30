namespace MyApp.Entities.Models;

/// <summary>
/// Representa a un usuario en el sistema.
/// </summary>
public class User : IAuditableEntity
{
    /// <summary>
    /// Identificador único del usuario.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre completo del usuario.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Dirección de correo electrónico del usuario, utilizada para el inicio de sesión y notificaciones.
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    /// <summary>
    /// Hash de la contraseña del usuario. Nunca se almacena la contraseña en texto plano.
    /// </summary>
    [Required]
    public string PasswordHash { get; set; }

    /// <summary>
    /// Clave foránea que referencia al rol asignado al usuario.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Indica si la cuenta de usuario está activa o deshabilitada.
    /// Por defecto, una nueva cuenta está activa.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // --- Campos de Auditoría (Implementación de IAuditableEntity) ---

    /// <summary>
    /// Fecha y hora en que se creó la cuenta de usuario.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Fecha y hora de la última modificación de la cuenta.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// El usuario o proceso que creó esta cuenta.
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// El último usuario o proceso que modificó esta cuenta.
    /// </summary>
    public string? ModifiedBy { get; set; }

    // --- Propiedades de Navegación ---

    /// <summary>
    /// Propiedad de navegación hacia la entidad Role.
    /// </summary>
    public virtual Role Role { get; set; }

    /// <summary>
    /// Colección de préstamos asociados a este usuario.
    /// </summary>
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
