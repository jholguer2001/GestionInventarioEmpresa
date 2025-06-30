namespace MyApp.Presentation.Models;
public class CreateItemViewModel
{
    [Required(ErrorMessage = "El código es requerido")]
    [StringLength(20, ErrorMessage = "El código no puede tener más de 20 caracteres")]
    [Display(Name = "Código")]
    public string Code { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
    [Display(Name = "Nombre")]
    public string Name { get; set; }

    [Required(ErrorMessage = "La categoría es requerida")]
    [StringLength(50, ErrorMessage = "La categoría no puede tener más de 50 caracteres")]
    [Display(Name = "Categoría")]
    public string Category { get; set; }

    [Required(ErrorMessage = "El estado es requerido")]
    [Display(Name = "Estado")]
    public ItemStatus Status { get; set; }

    [StringLength(100, ErrorMessage = "La ubicación no puede tener más de 100 caracteres")]
    [Display(Name = "Ubicación")]
    public string Location { get; set; }
}