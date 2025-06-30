namespace MyApp.DataAccess.Repositories.Interfaces;

/// <summary>
/// Define un contrato genérico para los repositorios, proporcionando
/// un conjunto estándar de métodos de acceso a datos.
/// </summary>
/// <typeparam name="T">El tipo de la entidad para la que es este repositorio.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Obtiene una entidad por su identificador único.
    /// </summary>
    /// <param name="id">El ID de la entidad a buscar.</param>
    /// <returns>La entidad encontrada, o nulo si no existe.</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Obtiene todas las entidades de un tipo.
    /// </summary>
    /// <returns>Una colección de todas las entidades.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Encuentra entidades que coinciden con un predicado (condición) específico.
    /// </summary>
    /// <param name="predicate">La expresión de filtro a aplicar.</param>
    /// <returns>Una colección de entidades que cumplen la condición.</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Obtiene la primera y única entidad que coincide con un predicado.
    /// </summary>
    /// <param name="predicate">La expresión de filtro a aplicar.</param>
    /// <returns>La entidad encontrada, o nulo si no se encuentra ninguna.</returns>
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Agrega una nueva entidad a la base de datos.
    /// </summary>
    /// <param name="entity">La entidad a agregar.</param>
    /// <returns>La entidad agregada (a menudo con el ID actualizado).</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Actualiza una entidad existente en la base de datos.
    /// </summary>
    /// <param name="entity">La entidad con los valores actualizados.</param>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Elimina una entidad de la base de datos por su ID.
    /// </summary>
    /// <param name="id">El ID de la entidad a eliminar.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Verifica si una entidad con el ID especificado existe.
    /// </summary>
    /// <param name="id">El ID de la entidad a verificar.</param>
    /// <returns>True si la entidad existe; de lo contrario, false.</returns>
    Task<bool> ExistsAsync(int id);

    /// <summary>
    /// Cuenta el número total de entidades.
    /// </summary>
    /// <returns>El número total de entidades.</returns>
    Task<int> CountAsync();

    /// <summary>
    /// Obtiene un subconjunto paginado de entidades.
    /// </summary>
    /// <param name="pageNumber">El número de página a recuperar (base 1).</param>
    /// <param name="pageSize">El número de entidades por página.</param>
    /// <returns>Una colección de entidades para la página especificada.</returns>
    Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
}