namespace MyApp.Presentation.Models;
 public class DashboardViewModel
{
    public int TotalItems { get; set; }
    public int AvailableItems { get; set; }
    public int ItemsOnLoan { get; set; }
    public int TotalUsers { get; set; }
    public int ActiveLoans { get; set; }
    public int PendingLoans { get; set; }
    public List<LoanDto> RecentLoans { get; set; } = new List<LoanDto>();
    public IEnumerable<LoanDto> OverdueLoans { get; set; } = new List<LoanDto>();
}

