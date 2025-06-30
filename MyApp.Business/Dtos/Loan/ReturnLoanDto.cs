namespace MyApp.Business.Dtos.Loan;

/// <summary>
/// DTO que contiene los datos para registrar la devolución de un préstamo.
/// </summary>
public class ReturnLoanDto
{
    /// <summary>
    /// El ID del préstamo que se está devolviendo.
    /// </summary>
    [Required(ErrorMessage = "El ID del préstamo es obligatorio.")]
    public int LoanId { get; set; }

    /// <summary>
    /// La fecha y hora exactas en que se realizó la devolución.
    /// </summary>
    [Required(ErrorMessage = "La fecha de devolución es obligatoria.")]
    public DateTime ReturnDate { get; set; }

    /// <summary>
    /// Comentarios opcionales sobre el estado del artículo devuelto o cualquier otra observación.
    /// </summary>
    [StringLength(500, ErrorMessage = "Los comentarios no pueden exceder los 500 caracteres.")]
    public string? Comments { get; set; }
}
