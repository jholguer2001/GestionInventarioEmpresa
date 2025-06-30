namespace MyApp.DataAccess.Repositories.Implementations;

/// <summary>
/// Implementación del repositorio para la entidad <see cref="Loan"/>.
/// </summary>
public class LoanRepository : Repository<Loan>, ILoanRepository
{
    /// <summary>
    /// Inicializa una nueva instancia del repositorio de Loan.
    /// </summary>
    /// <param name="context">El contexto de la base de datos inyectado.</param>
    public LoanRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene un préstamo por su ID, incluyendo el Usuario, Rol del usuario y el Artículo asociado.
    /// </summary>
    public override async Task<Loan?> GetByIdAsync(int id)
    {
        return await _context.Loans
            .Include(l => l.User)
                .ThenInclude(u => u.Role)
            .Include(l => l.Item)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    /// <summary>
    /// Obtiene todos los préstamos, incluyendo los datos del Usuario y el Artículo,
    /// ordenados por la fecha de solicitud más reciente.
    /// </summary>
    public override async Task<IEnumerable<Loan>> GetAllAsync()
    {
        return await _context.Loans
            .Include(l => l.User) // Carga los datos del usuario asociado
            .Include(l => l.Item) // Carga los datos del artículo asociado
            .OrderByDescending(l => l.RequestDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Loan>> GetByUserAsync(int userId)
    {
        return await _context.Loans
            .Include(l => l.Item)
            .Include(l => l.User)
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.RequestDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Loan>> GetByItemAsync(int itemId)
    {
        return await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Item)
            .Where(l => l.ItemId == itemId)
            .OrderByDescending(l => l.RequestDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status)
    {
        return await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Item)
            .Where(l => l.Status == status)
            .OrderByDescending(l => l.RequestDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Loan>> GetActiveLoansByItemAsync(int itemId)
    {
        return await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Item)
            .Where(l => l.ItemId == itemId &&
                         (l.Status == LoanStatus.Pending ||
                          l.Status == LoanStatus.Approved ||
                          l.Status == LoanStatus.Delivered))
            .OrderByDescending(l => l.RequestDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Loan?> GetCurrentLoanByItemAsync(int itemId)
    {
        return await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Item)
            .Where(l => l.ItemId == itemId && l.Status == LoanStatus.Delivered)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Loan>> GetPendingLoansAsync()
    {
        return await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Item)
            .Where(l => l.Status == LoanStatus.Pending)
            .OrderBy(l => l.RequestDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
    {
        var today = DateTime.UtcNow;
        var overdueThreshold = today.AddDays(-1); // Préstamos entregados hace más de 7 días

        return await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Item)
            .Where(l => l.Status == LoanStatus.Delivered &&    // Status = 3 (Entregados)
                       l.ReturnDate == null &&                 // NO han sido devueltos
                       l.DeliveryDate.HasValue &&              // Tienen fecha de entrega
                       l.DeliveryDate.Value < overdueThreshold) // Entregados hace más de 7 días
            .OrderBy(l => l.DeliveryDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Loan>> GetLoansByDateRangeAsync(DateTime fromDate, DateTime toDate)
    {
        return await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Item)
            .Where(l => l.RequestDate >= fromDate && l.RequestDate <= toDate)
            .OrderByDescending(l => l.RequestDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> HasActiveLoanAsync(int itemId)
    {
        return await _context.Loans
            .AnyAsync(l => l.ItemId == itemId &&
                           (l.Status == LoanStatus.Pending ||
                            l.Status == LoanStatus.Approved ||
                            l.Status == LoanStatus.Delivered));
    }

    /// <inheritdoc/>
    public async Task<int> GetActiveLoanCountByUserAsync(int userId)
    {
        return await _context.Loans
            .CountAsync(l => l.UserId == userId &&
                             (l.Status == LoanStatus.Pending ||
                              l.Status == LoanStatus.Approved ||
                              l.Status == LoanStatus.Delivered));
    }
}
 