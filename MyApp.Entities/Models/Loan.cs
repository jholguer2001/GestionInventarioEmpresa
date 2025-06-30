namespace MyApp.Entities.Models;

/// <summary>
/// Representa un registro de préstamo de un artículo a un usuario.
/// </summary>
public class Loan : IAuditableEntity
{
    /// <summary>
    /// Identificador único del registro de préstamo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Clave foránea que referencia al usuario que solicita el préstamo.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Clave foránea que referencia al artículo que es prestado.
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// Fecha y hora en que se solicitó el préstamo.
    /// </summary>
    public DateTime RequestDate { get; set; }

    /// <summary>
    /// Fecha y hora en que el artículo fue entregado al usuario.
    /// Es nulable, ya que puede no haber sido entregado aún.
    /// </summary>
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// Fecha y hora en que el artículo fue devuelto.
    /// Es nulable, ya que puede estar aún en posesión del usuario.
    /// </summary>
    public DateTime? ReturnDate { get; set; }

    /// <summary>
    /// Estado actual del préstamo (ej. Pendiente, Activo, Devuelto).
    /// El valor por defecto es "Pendiente".
    /// </summary>
    public LoanStatus Status { get; set; } = LoanStatus.Pending;

    /// <summary>
    /// Comentarios o notas adicionales sobre el préstamo.
    /// </summary>
    [StringLength(500)]
    public string? Comments { get; set; }

    // --- Campos de Auditoría (Implementación de IAuditableEntity) ---

    /// <summary>
    /// La fecha y hora en que se creó el registro del préstamo.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// La fecha y hora de la última modificación del registro.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// El usuario que registró la transacción.
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// El último usuario que modificó la transacción.
    /// </summary>
    public string? ModifiedBy { get; set; }

    // --- Propiedades de Navegación ---

    /// <summary>
    /// Propiedad de navegación hacia la entidad User.
    /// </summary>
    public virtual User User { get; set; }

    /// <summary>
    /// Propiedad de navegación hacia la entidad Item.
    /// </summary>
    public virtual Item Item { get; set; }
}