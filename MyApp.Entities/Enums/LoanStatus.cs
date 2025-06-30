namespace MyApp.Entities.Enums;

/// <summary>
/// Define los posibles estados en el ciclo de vida de un préstamo (Loan).
/// </summary>
public enum LoanStatus
{
    /// <summary>
    /// El préstamo ha sido solicitado por un usuario pero aún no ha sido revisado o procesado.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// La solicitud de préstamo ha sido aprobada. El artículo está reservado y listo para ser entregado.
    /// </summary>
    Approved = 1,

    /// <summary>
    /// La solicitud de préstamo ha sido denegada.
    /// </summary>
    Rejected = 2,

    /// <summary>
    /// El artículo ha sido entregado al usuario y el préstamo está activo.
    /// </summary>
    Delivered = 3,

    /// <summary>
    /// El usuario ha devuelto el artículo, completando el ciclo del préstamo.
    /// </summary>
    Returned = 4
}
