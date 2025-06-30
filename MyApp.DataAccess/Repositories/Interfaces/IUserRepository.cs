namespace MyApp.DataAccess.Repositories.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de usuarios (Users),
/// extendiendo las operaciones CRUD básicas con consultas específicas.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Obtiene un usuario por su dirección de correo electrónico única.
    /// </summary>
    /// <param name="email">El email del usuario a buscar.</param>
    /// <returns>El usuario encontrado, o nulo si no existe.</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Obtiene todos los usuarios que están asignados a un rol específico.
    /// </summary>
    /// <param name="roleId">El ID del rol.</param>
    /// <returns>Una colección de usuarios asignados al rol especificado.</returns>
    Task<IEnumerable<User>> GetByRoleAsync(int roleId);

    /// <summary>
    /// Verifica si ya existe un usuario con la dirección de correo electrónico especificada.
    /// </summary>
    /// <param name="email">El email a verificar.</param>
    /// <returns>True si el email ya está en uso; de lo contrario, false.</returns>
    Task<bool> EmailExistsAsync(string email);

    /// <summary>
    /// Obtiene todos los usuarios que están marcados como activos en el sistema.
    /// </summary>
    /// <returns>Una colección de usuarios activos.</returns>
    Task<IEnumerable<User>> GetActiveUsersAsync();

    /// <summary>
    /// Verifica si un correo electrónico ya está en uso por otro usuario diferente.
    /// Útil al actualizar un usuario para evitar duplicar correos.
    /// </summary>
    /// <param name="email">El email a verificar.</param>
    /// <param name="userId">El ID del usuario que se está editando.</param>
    /// <returns>True si el email existe en otro usuario; de lo contrario, false.</returns>
    Task<bool> EmailExistsForOtherUserAsync(string email, int userId);

    /// <summary>
    /// Busca usuarios cuyo nombre o email contenga el término de búsqueda.
    /// </summary>
    /// <param name="searchTerm">El término a buscar.</param>
    /// <returns>Una colección de usuarios que coinciden con la búsqueda.</returns>
    Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
}