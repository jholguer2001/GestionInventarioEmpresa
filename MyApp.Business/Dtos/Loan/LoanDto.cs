namespace MyApp.Business.Dtos.Loan;

/// <summary>
/// DTO que representa los datos de un préstamo para ser mostrados al cliente.
/// </summary>
public class LoanDto
{
    /// <summary>
    /// El identificador único del préstamo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El ID del usuario que solicitó el préstamo.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// El nombre del usuario que solicitó el préstamo.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// El ID del artículo prestado.
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// El nombre del artículo prestado.
    /// </summary>
    public string ItemName { get; set; }

    /// <summary>
    /// El código del artículo prestado.
    /// </summary>
    public string ItemCode { get; set; }

    /// <summary>
    /// La fecha en que se solicitó el préstamo.
    /// </summary>
    public DateTime RequestDate { get; set; }

    /// <summary>
    /// La fecha en que se entregó el artículo.
    /// </summary>
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// La fecha en que se devolvió el artículo.
    /// </summary>
    public DateTime? ReturnDate { get; set; }

    /// <summary>
    /// El estado actual del préstamo (ej. Pendiente, Activo, Devuelto).
    /// </summary>
    public LoanStatus Status { get; set; }

    /// <summary>
    /// Comentarios o notas adicionales sobre el préstamo.
    /// </summary>
    public string? Comments { get; set; }
}