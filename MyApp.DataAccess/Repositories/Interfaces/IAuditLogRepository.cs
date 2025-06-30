namespace MyApp.DataAccess.Repositories.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de registros de auditoría,
/// extendiendo las operaciones CRUD básicas con consultas específicas de auditoría.
/// </summary>
public interface IAuditLogRepository : IRepository<AuditLog>
{
    /// <summary>
    /// Obtiene todos los registros de auditoría para un nombre de tabla específico.
    /// </summary>
    /// <param name="tableName">El nombre de la tabla a consultar.</param>
    /// <returns>Una colección de registros de auditoría.</returns>
    Task<IEnumerable<AuditLog>> GetByTableAsync(string tableName);

    /// <summary>
    /// Obtiene todos los registros de auditoría realizados por un usuario específico.
    /// </summary>
    /// <param name="username">El identificador del usuario.</param>
    /// <returns>Una colección de registros de auditoría.</returns>
    Task<IEnumerable<AuditLog>> GetByUserAsync(string username);

    /// <summary>
    /// Obtiene todos los registros de auditoría dentro de un rango de fechas específico.
    /// </summary>
    /// <param name="fromDate">La fecha de inicio del rango.</param>
    /// <param name="toDate">La fecha de fin del rango.</param>
    /// <returns>Una colección de registros de auditoría.</returns>
    Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Obtiene el historial completo de cambios para un registro específico en una tabla.
    /// </summary>
    /// <param name="tableName">El nombre de la tabla.</param>
    /// <param name="primaryKey">La clave primaria del registro a consultar.</param>
    /// <returns>Una colección de registros de auditoría para el registro específico.</returns>
    Task<IEnumerable<AuditLog>> GetByTableAndPrimaryKeyAsync(string tableName, string primaryKey);
}
