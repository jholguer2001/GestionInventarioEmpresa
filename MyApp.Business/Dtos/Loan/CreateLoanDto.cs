using System.ComponentModel.DataAnnotations;

namespace MyApp.Business.Dtos.Loan;

/// <summary>
/// DTO que contiene los datos necesarios para crear una nueva solicitud de préstamo.
/// </summary>
public class CreateLoanDto
{
    /// <summary>
    /// El ID del artículo que se está solicitando.
    /// </summary>
    [Required(ErrorMessage = "Debe seleccionar un artículo.")]
    public int ItemId { get; set; }

    /// <summary>
    /// La fecha en que el usuario desea que se le entregue el artículo. Es opcional.
    /// </summary>
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// Comentarios o notas adicionales del usuario al realizar la solicitud.
    /// </summary>
    [StringLength(500, ErrorMessage = "Los comentarios no pueden exceder los 500 caracteres.")]
    public string? Comments { get; set; }
}
