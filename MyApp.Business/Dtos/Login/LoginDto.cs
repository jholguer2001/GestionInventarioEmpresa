namespace MyApp.Business.Dtos.Login;

/// <summary>
/// DTO que contiene las credenciales para el inicio de sesión de un usuario.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// La dirección de correo electrónico del usuario.
    /// </summary>
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; }

    /// <summary>
    /// La contraseña del usuario.
    /// </summary>
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; }

    /// <summary>

    /// </summary>
    public bool RememberMe { get; set; }
}