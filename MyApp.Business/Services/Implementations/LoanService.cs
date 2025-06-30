namespace MyApp.Business.Services.Implementations;

/// <summary>
/// Implementación del servicio de negocio para la gestión de préstamos (Loans).
/// </summary>
public class LoanService : ILoanService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditService _auditService;
    private readonly IItemService _itemService;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de préstamos.
    /// </summary>
    /// <param name="unitOfWork">La unidad de trabajo para el acceso a datos.</param>
    /// <param name="auditService">El servicio para registrar acciones de auditoría.</param>
    /// <param name="itemService">El servicio para la lógica de negocio de los artículos.</param>
    public LoanService(IUnitOfWork unitOfWork, IAuditService auditService, IItemService itemService)
    {
        _unitOfWork = unitOfWork;
        _auditService = auditService;
        _itemService = itemService;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LoanDto>> GetAllLoansAsync()
    {
        var loans = await _unitOfWork.Loans.GetAllAsync();
        return loans.Select(MapToDto);
    }

    /// <inheritdoc/>
    public async Task<LoanDto> GetLoanByIdAsync(int id)
    {
        var loan = await _unitOfWork.Loans.GetByIdAsync(id);
        if (loan == null)
            throw new KeyNotFoundException($"Loan with ID {id} not found");

        return MapToDto(loan);
    }

    /// <inheritdoc/>
    public async Task<LoanDto> CreateLoanAsync(int userId, CreateLoanDto createLoanDto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null || !user.IsActive)
            throw new KeyNotFoundException($"Active user with ID {userId} not found");

        if (!await _itemService.IsItemAvailableForLoanAsync(createLoanDto.ItemId))
            throw new InvalidOperationException("Item is not available for loan");

        var loan = new Loan
        {
            UserId = userId,
            ItemId = createLoanDto.ItemId,
            RequestDate = DateTime.UtcNow,
            DeliveryDate = createLoanDto.DeliveryDate,
            Status = LoanStatus.Pending,
            Comments = createLoanDto.Comments
        };
        await _unitOfWork.Loans.AddAsync(loan);

        var item = await _unitOfWork.Items.GetByIdAsync(createLoanDto.ItemId);
        item.Status = ItemStatus.OnLoan;
        await _unitOfWork.Items.UpdateAsync(item);

        await _unitOfWork.SaveChangesAsync();
        loan = await _unitOfWork.Loans.GetByIdAsync(loan.Id);

        await _auditService.LogActionAsync("Loans", "CREATE", loan.Id.ToString(), null,
            new { loan.UserId, UserName = loan.User.Name, loan.ItemId, ItemName = loan.Item.Name, loan.RequestDate, loan.Status },
            "Loan request created");

        return MapToDto(loan);
    }

    /// <inheritdoc/>
    public async Task<bool> ApproveLoanAsync(ApproveLoanDto approveLoanDto)
    {
        var loan = await _unitOfWork.Loans.GetByIdAsync(approveLoanDto.LoanId);
        if (loan == null) return false;

        if (loan.Status != LoanStatus.Pending)
            throw new InvalidOperationException("Only pending loans can be approved or rejected");

        var oldStatus = loan.Status;
        loan.Status = approveLoanDto.Approved ? LoanStatus.Approved : LoanStatus.Rejected;
        loan.Comments = string.IsNullOrEmpty(loan.Comments) ? approveLoanDto.Comments : $"{loan.Comments}\n{approveLoanDto.Comments}";

        if (!approveLoanDto.Approved)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(loan.ItemId);
            item.Status = ItemStatus.Available;
            await _unitOfWork.Items.UpdateAsync(item);
        }

        await _unitOfWork.Loans.UpdateAsync(loan);
        await _unitOfWork.SaveChangesAsync();

        await _auditService.LogActionAsync("Loans", approveLoanDto.Approved ? "APPROVE" : "REJECT", loan.Id.ToString(),
            new { OldStatus = oldStatus }, new { NewStatus = loan.Status, approveLoanDto.Comments },
            $"Loan {(approveLoanDto.Approved ? "approved" : "rejected")}");

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> DeliverLoanAsync(int loanId)
    {
        var loan = await _unitOfWork.Loans.GetByIdAsync(loanId);
        if (loan == null) return false;

        if (loan.Status != LoanStatus.Approved)
            throw new InvalidOperationException("Only approved loans can be delivered");

        var oldStatus = loan.Status;
        loan.Status = LoanStatus.Delivered;
        loan.DeliveryDate = DateTime.UtcNow;

        await _unitOfWork.Loans.UpdateAsync(loan);
        await _unitOfWork.SaveChangesAsync();

        await _auditService.LogActionAsync("Loans", "DELIVER", loan.Id.ToString(),
            new { OldStatus = oldStatus }, new { NewStatus = LoanStatus.Delivered, DeliveryDate = loan.DeliveryDate },
            "Item delivered to user");

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> ReturnLoanAsync(ReturnLoanDto returnLoanDto)
    {
        var loan = await _unitOfWork.Loans.GetByIdAsync(returnLoanDto.LoanId);
        if (loan == null) return false;

        if (loan.Status != LoanStatus.Delivered)
            throw new InvalidOperationException("Only delivered loans can be returned");

        var oldValues = new { loan.Status, loan.ReturnDate };
        loan.Status = LoanStatus.Returned;
        loan.ReturnDate = returnLoanDto.ReturnDate;
        loan.Comments = string.IsNullOrEmpty(loan.Comments) ? returnLoanDto.Comments : $"{loan.Comments}\n{returnLoanDto.Comments}";

        var item = await _unitOfWork.Items.GetByIdAsync(loan.ItemId);
        item.Status = ItemStatus.Available;
        await _unitOfWork.Items.UpdateAsync(item);

        await _unitOfWork.Loans.UpdateAsync(loan);
        await _unitOfWork.SaveChangesAsync();

        await _auditService.LogActionAsync("Loans", "RETURN", loan.Id.ToString(), oldValues,
            new { loan.Status, loan.ReturnDate, returnLoanDto.Comments },
            "Item returned");

        return true;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LoanDto>> GetLoansByUserAsync(int userId)
    {
        var loans = await _unitOfWork.Loans.GetByUserAsync(userId);
        return loans.Select(MapToDto);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LoanDto>> GetPendingLoansAsync()
    {
        var loans = await _unitOfWork.Loans.GetPendingLoansAsync();
        return loans.Select(MapToDto);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LoanDto>> GetActiveLoansByItemAsync(int itemId)
    {
        var loans = await _unitOfWork.Loans.GetActiveLoansByItemAsync(itemId);
        return loans.Select(MapToDto);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LoanDto>> GetOverdueLoansAsync()
    {
        var loans = await _unitOfWork.Loans.GetOverdueLoansAsync();
        return loans.Select(MapToDto);
    }

    /// <summary>
    /// Mapea una entidad Loan a su correspondiente LoanDto.
    /// </summary>
    private static LoanDto MapToDto(Loan loan)
    {
        return new LoanDto
        {
            Id = loan.Id,
            UserId = loan.UserId,
            UserName = loan.User?.Name ?? "Unknown",
            ItemId = loan.ItemId,
            ItemName = loan.Item?.Name ?? "Unknown",
            ItemCode = loan.Item?.Code ?? "Unknown",
            RequestDate = loan.RequestDate,
            DeliveryDate = loan.DeliveryDate,
            ReturnDate = loan.ReturnDate,
            Status = loan.Status,
            Comments = loan.Comments
        };
    }
}
