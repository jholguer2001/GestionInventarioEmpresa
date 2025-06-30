namespace MyApp.DataAccess.UnitOfWork;

/// <summary>
/// Implementación del patrón de Unidad de Trabajo (Unit of Work).
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    // Campos privados para la inicialización perezosa de repositorios
    private IUserRepository? _users;
    private IRoleRepository? _roles;
    private IItemRepository? _items;
    private ILoanRepository? _loans;
    private IAuditLogRepository? _auditLogs;

    /// <summary>
    /// Inicializa una nueva instancia de la Unidad de Trabajo.
    /// </summary>
    /// <param name="context">El contexto de la base de datos inyectado.</param>
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public IUserRepository Users => _users ??= new UserRepository(_context);

    /// <inheritdoc/>
    public IRoleRepository Roles => _roles ??= new RoleRepository(_context);

    /// <inheritdoc/>
    public IItemRepository Items => _items ??= new ItemRepository(_context);

    /// <inheritdoc/>
    public ILoanRepository Loans => _loans ??= new LoanRepository(_context);

    /// <inheritdoc/>
    public IAuditLogRepository AuditLogs => _auditLogs ??= new AuditLogRepository(_context);

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    /// <inheritdoc/>
    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <inheritdoc/>
    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Libera los recursos utilizados por el contexto y la transacción.
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }
}