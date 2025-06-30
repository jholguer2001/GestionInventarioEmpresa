namespace MyApp.Business.Services.Interfaces;

/// <summary>
/// Define un contrato para el servicio de auditoría, que maneja la lógica
/// de negocio para registrar y consultar eventos del sistema.
/// </summary>
public interface IAuditService
{
    /// <summary>
    /// Registra una acción de auditoría en el sistema.
    /// </summary>
    /// <param name="tableName">El nombre de la tabla afectada.</param>
    /// <param name="action">La acción realizada (ej. "CREATE", "UPDATE", "DELETE").</param>
    /// <param name="primaryKey">La clave primaria del registro afectado.</param>
    /// <param name="oldValues">Un objeto que representa los valores del registro antes del cambio (para UPDATE/DELETE). Será serializado a JSON.</param>
    /// <param name="newValues">Un objeto que representa los valores del registro después del cambio (para CREATE/UPDATE). Será serializado a JSON.</param>
    /// <param name="description">Una descripción opcional legible por humanos sobre la acción.</param>
    Task LogActionAsync(string tableName, string action, string primaryKey,
                        object oldValues, object newValues, string? description = null);

    /// <summary>
    /// Obtiene el historial completo de auditoría para un registro específico.
    /// </summary>
    /// <param name="tableName">El nombre de la tabla del registro.</param>
    /// <param name="primaryKey">La clave primaria del registro.</param>
    /// <returns>Una colección de registros de auditoría que trazan el historial del registro.</returns>
    Task<IEnumerable<AuditLog>> GetAuditTrailAsync(string tableName, string primaryKey);

    /// <summary>
    /// Obtiene toda la actividad de un usuario específico, opcionalmente filtrada por un rango de fechas.
    /// </summary>
    /// <param name="username">El identificador del usuario.</param>
    /// <param name="fromDate">La fecha de inicio opcional del filtro.</param>
    /// <param name="toDate">La fecha de fin opcional del filtro.</param>
    /// <returns>Una colección de registros de auditoría realizados por el usuario.</returns>
    Task<IEnumerable<AuditLog>> GetUserActivityAsync(string username, DateTime? fromDate = null, DateTime? toDate = null);

    /// <summary>
    /// Obtiene toda la actividad del sistema dentro de un rango de fechas.
    /// </summary>
    /// <param name="fromDate">La fecha de inicio del rango.</param>
    /// <param name="toDate">La fecha de fin del rango.</param>
    /// <returns>Una colección de todos los registros de auditoría en el período especificado.</returns>
    Task<IEnumerable<AuditLog>> GetSystemActivityAsync(DateTime fromDate, DateTime toDate);
}