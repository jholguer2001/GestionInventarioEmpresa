namespace MyApp.Business.Services.Interfaces;

public interface ILoanService
{
    Task<IEnumerable<LoanDto>> GetAllLoansAsync();
    Task<LoanDto> GetLoanByIdAsync(int id);
    Task<LoanDto> CreateLoanAsync(int userId, CreateLoanDto createLoanDto);
    Task<bool> ApproveLoanAsync(ApproveLoanDto approveLoanDto);
    Task<bool> ReturnLoanAsync(ReturnLoanDto returnLoanDto);
    Task<IEnumerable<LoanDto>> GetLoansByUserAsync(int userId);
    Task<IEnumerable<LoanDto>> GetPendingLoansAsync();
    Task<IEnumerable<LoanDto>> GetActiveLoansByItemAsync(int itemId);
    Task<IEnumerable<LoanDto>> GetOverdueLoansAsync();
    Task<bool> DeliverLoanAsync(int loanId);
}
