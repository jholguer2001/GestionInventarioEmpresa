namespace MyApp.Business.Dtos.User;

/// <summary>
/// DTO que contiene los datos necesarios para crear un nuevo usuario.
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// El nombre completo del nuevo usuario.
    /// </summary>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string Name { get; set; }

    /// <summary>
    /// La dirección de correo electrónico del nuevo usuario. Debe ser única en el sistema.
    /// </summary>
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; }

    /// <summary>
    /// La contraseña para la nueva cuenta.
    /// </summary>
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres.")]
    public string Password { get; set; }

    /// <summary>
    /// El ID del rol que se asignará al nuevo usuario.
    /// </summary>
    [Required(ErrorMessage = "Debe asignar un rol al usuario.")]
    public int RoleId { get; set; }
}