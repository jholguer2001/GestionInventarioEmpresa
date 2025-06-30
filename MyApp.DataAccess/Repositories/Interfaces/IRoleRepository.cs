namespace MyApp.DataAccess.Repositories.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de roles (Roles),
/// extendiendo las operaciones CRUD básicas con consultas específicas.
/// </summary>
public interface IRoleRepository : IRepository<Role>
{
    /// <summary>
    /// Obtiene un rol por su nombre único.
    /// </summary>
    /// <param name="name">El nombre del rol a buscar.</param>
    /// <returns>El rol encontrado, o nulo si no existe.</returns>
    Task<Role?> GetByNameAsync(string name);

    /// <summary>
    /// Obtiene todos los roles que están actualmente activos en el sistema.
    /// </summary>
    /// <returns>Una colección de roles activos.</returns>
    Task<IEnumerable<Role>> GetActiveRolesAsync();

    /// <summary>
    /// Verifica si ya existe un rol con el nombre especificado.
    /// </summary>
    /// <param name="name">El nombre del rol a verificar.</param>
    /// <returns>True si el rol ya existe; de lo contrario, false.</returns>
    Task<bool> RoleExistsAsync(string name);

    /// <summary>
    /// Verifica si un rol tiene al menos un usuario asignado.
    /// </summary>
    /// <param name="roleId">El ID del rol a verificar.</param>
    /// <returns>True si el rol tiene usuarios asignados; de lo contrario, false.</returns>
    Task<bool> RoleHasUsersAsync(int roleId);

    /// <summary>
    /// Cuenta el número de usuarios que tienen un rol específico asignado.
    /// </summary>
    /// <param name="roleId">El ID del rol.</param>
    /// <returns>El número total de usuarios asignados a ese rol.</returns>
    Task<int> GetUserCountByRoleAsync(int roleId);
}
