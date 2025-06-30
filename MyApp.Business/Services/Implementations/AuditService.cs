namespace MyApp.Business.Services.Implementations;

/// <summary>
/// Implementación del servicio de auditoría.
/// </summary>
public class AuditService : IAuditService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de auditoría.
    /// </summary>
    /// <param name="unitOfWork">La unidad de trabajo para acceder a los repositorios.</param>
    /// <param name="currentUserService">El servicio para obtener información del usuario actual.</param>
    public AuditService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    /// <inheritdoc/>
    public async Task LogActionAsync(string tableName, string action, string primaryKey,
                                    object oldValues, object newValues, string? description = null)
    {
        var auditLog = new AuditLog
        {
            TableName = tableName,
            Action = action,
            PrimaryKey = primaryKey,
            // Serializa los objetos a JSON para un almacenamiento flexible.
            OldValues = oldValues != null ? JsonConvert.SerializeObject(oldValues) : null,
            NewValues = newValues != null ? JsonConvert.SerializeObject(newValues) : null,
            ActionDate = DateTime.UtcNow, // La BD usará GETDATE(), pero es bueno tenerlo aquí.
            ActionBy = _currentUserService.GetCurrentUserEmail() ?? "System",
            ActionDescription = description,
            IpAddress = _currentUserService.GetClientIpAddress(),
            UserAgent = _currentUserService.GetUserAgent()
        };

        await _unitOfWork.AuditLogs.AddAsync(auditLog);
        // La llamada a SaveChangesAsync se hace aquí para asegurar que el log se guarda inmediatamente.
        // En otros escenarios, podría delegarse a una capa superior.
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AuditLog>> GetAuditTrailAsync(string tableName, string primaryKey)
    {
        return await _unitOfWork.AuditLogs.GetByTableAndPrimaryKeyAsync(tableName, primaryKey);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AuditLog>> GetUserActivityAsync(string username, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = await _unitOfWork.AuditLogs.GetByUserAsync(username);

        if (fromDate.HasValue)
            query = query.Where(a => a.ActionDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(a => a.ActionDate <= toDate.Value);

        return query.OrderByDescending(a => a.ActionDate);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AuditLog>> GetSystemActivityAsync(DateTime fromDate, DateTime toDate)
    {
        return await _unitOfWork.AuditLogs.GetByDateRangeAsync(fromDate, toDate);
    }
}