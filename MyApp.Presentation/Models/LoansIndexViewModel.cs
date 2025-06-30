

namespace MyApp.Presentation.Models;

public class LoansIndexViewModel
{
    public List<LoanDto> Loans { get; set; } = new List<LoanDto>();
    public LoanStatus? SelectedStatus { get; set; }
}
