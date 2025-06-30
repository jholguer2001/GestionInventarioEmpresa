namespace MyApp.Business.Dtos.Role;

/// <summary>
/// DTO que representa los datos de un rol para ser mostrados al cliente.
/// </summary>
public class RoleDto
{
    /// <summary>
    /// El identificador único del rol.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El nombre del rol.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// La descripción opcional del rol.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// El número total de usuarios que tienen este rol asignado.
    /// </summary>
    public int UserCount { get; set; }
}
