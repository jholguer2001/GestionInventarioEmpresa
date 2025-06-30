namespace MyApp.Business.Services.Interfaces;

/// <summary>
/// Define un contrato para el servicio de negocio que maneja la lógica de los usuarios.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Obtiene todos los usuarios registrados en el sistema.
    /// </summary>
    /// <returns>Una colección de DTOs de usuarios.</returns>
    Task<IEnumerable<UserDto>> GetAllUsersAsync();

    /// <summary>
    /// Obtiene un usuario específico por su ID.
    /// </summary>
    /// <param name="id">El ID del usuario a buscar.</param>
    /// <returns>Un DTO del usuario encontrado, o nulo si no existe.</returns>
    Task<UserDto?> GetUserByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="createUserDto">El DTO con los datos para crear el usuario.</param>
    /// <returns>Un DTO del usuario recién creado.</returns>
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);

    /// <summary>
    /// Actualiza un usuario existente.
    /// </summary>
    /// <param name="id">El ID del usuario a actualizar.</param>
    /// <param name="updateUserDto">El DTO con los nuevos datos del usuario.</param>
    /// <returns>Un DTO del usuario actualizado.</returns>
    Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);

    /// <summary>
    /// Elimina un usuario del sistema.
    /// </summary>
    /// <param name="id">El ID del usuario a eliminar.</param>
    /// <returns>True si la eliminación fue exitosa; de lo contrario, false.</returns>
    Task<bool> DeleteUserAsync(int id);

    /// <summary>
    /// Cambia el rol de un usuario existente.
    /// </summary>
    /// <param name="userId">El ID del usuario cuyo rol se cambiará.</param>
    /// <param name="roleId">El ID del nuevo rol a asignar.</param>
    /// <returns>True si el cambio de rol fue exitoso; de lo contrario, false.</returns>
    Task<bool> ChangeUserRoleAsync(int userId, int roleId);

    /// <summary>
    /// Obtiene todos los usuarios que están asignados a un rol específico.
    /// </summary>
    /// <param name="roleId">El ID del rol.</param>
    /// <returns>Una colección de DTOs de usuarios asignados al rol especificado.</returns>
    Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId);
}