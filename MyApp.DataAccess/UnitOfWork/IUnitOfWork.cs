namespace MyApp.DataAccess.UnitOfWork;

/// <summary>
/// Define un contrato para el patrón de Unidad de Trabajo (Unit of Work),
/// que gestiona las transacciones y coordina el trabajo entre múltiples repositorios.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Obtiene el repositorio de usuarios.
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Obtiene el repositorio de roles.
    /// </summary>
    IRoleRepository Roles { get; }

    /// <summary>
    /// Obtiene el repositorio de artículos.
    /// </summary>
    IItemRepository Items { get; }

    /// <summary>
    /// Obtiene el repositorio de préstamos.
    /// </summary>
    ILoanRepository Loans { get; }

    /// <summary>
    /// Obtiene el repositorio de registros de auditoría.
    /// </summary>
    IAuditLogRepository AuditLogs { get; }

    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos como una única transacción.
    /// </summary>
    /// <returns>El número de objetos de estado escritos en la base de datos.</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Inicia explícitamente una nueva transacción en la base de datos.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Confirma la transacción actual, haciendo que todos los cambios sean permanentes.
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Revierte la transacción actual, descartando todos los cambios pendientes.
    /// </summary>
    Task RollbackTransactionAsync();
}
