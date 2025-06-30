namespace MyApp.DataAccess.Repositories.Implementations;

/// <summary>
/// Implementación del repositorio para la entidad <see cref="AuditLog"/>.
/// </summary>
public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
{
    /// <summary>
    /// Inicializa una nueva instancia del repositorio de AuditLog.
    /// </summary>
    /// <param name="context">El contexto de la base de datos inyectado.</param>
    public AuditLogRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AuditLog>> GetByTableAsync(string tableName)
    {
        return await _context.AuditLogs
            .Where(a => a.TableName == tableName)
            .OrderByDescending(a => a.ActionDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AuditLog>> GetByUserAsync(string username)
    {
        return await _context.AuditLogs
            .Where(a => a.ActionBy == username)
            .OrderByDescending(a => a.ActionDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
    {
        return await _context.AuditLogs
            .Where(a => a.ActionDate >= fromDate && a.ActionDate <= toDate)
            .OrderByDescending(a => a.ActionDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AuditLog>> GetByTableAndPrimaryKeyAsync(string tableName, string primaryKey)
    {
        return await _context.AuditLogs
            .Where(a => a.TableName == tableName && a.PrimaryKey == primaryKey)
            .OrderByDescending(a => a.ActionDate)
            .ToListAsync();
    }
}