namespace MyApp.Presentation.Models;
public class CreateLoanViewModel
{
    [Required(ErrorMessage = "Por favor selecciona un artículo")]
    [Display(Name = "Artículo")]
    public int ItemId { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de entrega deseada")]
    public DateTime? DeliveryDate { get; set; }

    [StringLength(500, ErrorMessage = "Los comentarios no pueden tener más de 500 caracteres")]
    [Display(Name = "Comentarios")]
    public string Comments { get; set; }
}