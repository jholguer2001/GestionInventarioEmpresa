namespace MyApp.Business.Dtos.User;

/// <summary>
/// DTO que contiene los datos para actualizar un usuario existente.
/// </summary>
public class UpdateUserDto
{
    /// <summary>
    /// El nombre completo del usuario.
    /// </summary>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string Name { get; set; }

    /// <summary>
    /// La dirección de correo electrónico del usuario. Debe ser única.
    /// </summary>
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; }

    /// <summary>
    /// El ID del rol asignado al usuario.
    /// </summary>
    [Required(ErrorMessage = "Debe asignar un rol al usuario.")]
    public int RoleId { get; set; }

    /// <summary>
    /// Indica si la cuenta del usuario está activa o deshabilitada.
    /// </summary>
    [Required(ErrorMessage = "Debe especificar el estado de actividad del usuario.")]
    public bool IsActive { get; set; }
}
