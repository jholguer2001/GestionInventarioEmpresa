namespace MyApp.Business.Dtos.User;

/// <summary>
/// DTO que representa los datos de un usuario para ser mostrados al cliente.
/// </summary>
public class UserDto
{
    /// <summary>
    /// El identificador único del usuario.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El nombre completo del usuario.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// La dirección de correo electrónico del usuario.
    /// </summary>
    [Required]
    public string Email { get; set; }

    /// <summary>
    /// El nombre del rol asignado al usuario.
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// Indica si la cuenta del usuario está activa.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// La fecha y hora en que se creó la cuenta del usuario.
    /// </summary>
    public DateTime CreatedDate { get; set; }
}