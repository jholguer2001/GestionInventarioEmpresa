using System.ComponentModel.DataAnnotations;

namespace MyApp.Business.Dtos.Loan;

/// <summary>
/// DTO que contiene los datos para aprobar o rechazar un préstamo.
/// </summary>
public class ApproveLoanDto
{
    /// <summary>
    /// El ID del préstamo que se va a procesar.
    /// </summary>
    [Required(ErrorMessage = "El ID del préstamo es obligatorio.")]
    public int LoanId { get; set; }

    /// <summary>
    /// La decisión: 'true' para aprobar, 'false' para rechazar.
    /// </summary>
    [Required(ErrorMessage = "La decisión de aprobación/rechazo es obligatoria.")]
    public bool Approved { get; set; }

    /// <summary>
    /// Comentarios opcionales sobre la decisión (ej. motivo del rechazo).
    /// </summary>
    [StringLength(500, ErrorMessage = "Los comentarios no pueden exceder los 500 caracteres.")]
    public string? Comments { get; set; }
}