namespace MyApp.Presentation.Controllers;

/// <summary>
/// Gestiona las páginas principales de la aplicación, como el panel de control.
/// </summary>
[Authorize]
public class HomeController : Controller
{
    private readonly IItemService _itemService;
    private readonly ILoanService _loanService;
    private readonly IUserService _userService;
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del controlador Home.
    /// </summary>
    /// <param name="itemService">El servicio para la lógica de negocio de los artículos.</param>
    /// <param name="loanService">El servicio para la lógica de negocio de los préstamos.</param>
    /// <param name="userService">El servicio para la lógica de negocio de los usuarios.</param>
    /// <param name="logger">El servicio para registrar logs.</param>
    public HomeController(
        IItemService itemService,
        ILoanService loanService,
        IUserService userService,
        ILogger<HomeController> logger)
    {
        _itemService = itemService;
        _loanService = loanService;
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Muestra el panel de control principal con un resumen del estado del sistema.
    /// </summary>
    /// <returns>La vista del panel de control con los datos agregados.</returns>
    public async Task<IActionResult> Index()
    {
        try
        {
            var model = new DashboardViewModel();

            var allItems = await _itemService.GetAllItemsAsync();
            var allLoans = await _loanService.GetAllLoansAsync();
            var allUsers = await _userService.GetAllUsersAsync();

            model.TotalItems = allItems.Count();
            model.AvailableItems = allItems.Count(i => i.Status == MyApp.Entities.Enums.ItemStatus.Available);
            model.ItemsOnLoan = allItems.Count(i => i.Status == MyApp.Entities.Enums.ItemStatus.OnLoan);
            model.TotalUsers = allUsers.Count();
            model.ActiveLoans = allLoans.Count(l => l.Status == MyApp.Entities.Enums.LoanStatus.Delivered);
            model.PendingLoans = allLoans.Count(l => l.Status == MyApp.Entities.Enums.LoanStatus.Pending);

            model.RecentLoans = allLoans.OrderByDescending(l => l.RequestDate).Take(5).ToList();
            model.OverdueLoans = await _loanService.GetOverdueLoansAsync();

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando el panel de control");
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    /// <summary>
    /// Muestra la página de política de privacidad.
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Muestra una página de error genérica.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
