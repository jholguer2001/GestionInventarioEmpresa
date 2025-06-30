namespace MyApp.Presentation.Controllers;
[Authorize]
public class LoansController : Controller
{
    private readonly ILoanService _loanService;
    private readonly IItemService _itemService;
    private readonly IUserService _userService;
    private readonly ILogger<LoansController> _logger;

    public LoansController(
        ILoanService loanService,
        IItemService itemService,
        IUserService userService,
        ILogger<LoansController> logger)
    {
        _loanService = loanService;
        _itemService = itemService;
        _userService = userService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(LoanStatus? status)
    {
        try
        {
            IEnumerable<LoanDto> loans;

            if (User.IsInRole("Administrator"))
            {
                if (status.HasValue)
                {
                    loans = await _loanService.GetAllLoansAsync();
                    loans = loans.Where(l => l.Status == status.Value);
                }
                else
                {
                    loans = await _loanService.GetAllLoansAsync();
                }
            }
            else
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    loans = await _loanService.GetLoansByUserAsync(userId);
                    if (status.HasValue)
                    {
                        loans = loans.Where(l => l.Status == status.Value);
                    }
                }
                else
                {
                    loans = new List<LoanDto>();
                }
            }

            var model = new LoansIndexViewModel
            {
                Loans = loans.ToList(),
                SelectedStatus = status
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando préstamos");
            TempData["ErrorMessage"] = "Error cargando préstamos. Por favor, inténtalo de nuevo.";
            return View(new LoansIndexViewModel { Loans = new List<LoanDto>() });
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var loan = await _loanService.GetLoanByIdAsync(id);

            if (!User.IsInRole("Administrator"))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId) || loan.UserId != userId)
                {
                    return Forbid();
                }
            }

            return View(loan);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando detalles del préstamo para ID {LoanId}", id);
            return View("Error");
        }
    }

    public async Task<IActionResult> Create()
    {
        try
        {
            var allItems = await _itemService.GetAllItemsAsync();
            var availableItems = allItems.Where(i => i.Status == ItemStatus.Available).ToList();

            ViewBag.Items = new SelectList(availableItems, "Id", "Name");

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando página de crear préstamo");
            return View("Error");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLoanViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadItemsForCreate();
            return View(model);
        }

        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                ModelState.AddModelError(string.Empty, "No se pudo identificar el usuario actual.");
                await LoadItemsForCreate();
                return View(model);
            }

            var createDto = new CreateLoanDto
            {
                ItemId = model.ItemId,
                DeliveryDate = model.DeliveryDate,
                Comments = model.Comments
            };

            await _loanService.CreateLoanAsync(userId, createDto);
            _logger.LogInformation("Solicitud de préstamo creada por usuario {UserId} para artículo {ItemId}", userId, model.ItemId);
            TempData["SuccessMessage"] = "¡Solicitud de préstamo creada exitosamente! Esperando aprobación.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await LoadItemsForCreate();
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando solicitud de préstamo");
            ModelState.AddModelError(string.Empty, "Ocurrió un error al crear la solicitud de préstamo.");
            await LoadItemsForCreate();
            return View(model);
        }
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Pending()
    {
        try
        {
            var pendingLoans = await _loanService.GetPendingLoansAsync();
            return View(pendingLoans);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando préstamos pendientes");
            return View("Error");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id, bool approved, string comments)
    {
        try
        {
            var approveDto = new ApproveLoanDto
            {
                LoanId = id,
                Approved = approved,
                Comments = comments
            };

            var success = await _loanService.ApproveLoanAsync(approveDto);
            if (success)
            {
                _logger.LogInformation("Préstamo {LoanId} {Action} por administrador", id, approved ? "aprobado" : "rechazado");
                TempData["SuccessMessage"] = $"¡Préstamo {(approved ? "aprobado" : "rechazado")} exitosamente!";
            }
            else
            {
                TempData["ErrorMessage"] = "Préstamo no encontrado o no se puede procesar.";
            }

            return RedirectToAction(nameof(Pending));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando aprobación de préstamo para ID {LoanId}", id);
            TempData["ErrorMessage"] = "Ocurrió un error al procesar el préstamo.";
            return RedirectToAction(nameof(Pending));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deliver(int id)
    {
        try
        {
            var success = await _loanService.DeliverLoanAsync(id);
            if (success)
            {
                _logger.LogInformation("Préstamo {LoanId} marcado como entregado", id);
                TempData["SuccessMessage"] = "¡Préstamo marcado como entregado exitosamente!";
            }
            else
            {
                TempData["ErrorMessage"] = "Préstamo no encontrado o no se puede entregar.";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error entregando préstamo {LoanId}", id);
            TempData["ErrorMessage"] = "Ocurrió un error al entregar el préstamo.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(int id, string comments)
    {
        try
        {
            var returnDto = new ReturnLoanDto
            {
                LoanId = id,
                ReturnDate = DateTime.Now,
                Comments = comments
            };

            var success = await _loanService.ReturnLoanAsync(returnDto);
            if (success)
            {
                _logger.LogInformation("Préstamo {LoanId} devuelto", id);
                TempData["SuccessMessage"] = "¡Artículo devuelto exitosamente!";
            }
            else
            {
                TempData["ErrorMessage"] = "Préstamo no encontrado o no se puede devolver.";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error devolviendo préstamo {LoanId}", id);
            TempData["ErrorMessage"] = "Ocurrió un error al devolver el artículo.";
            return RedirectToAction(nameof(Index));
        }
    }



    /// <summary>
    /// Muestra la lista de préstamos vencidos. Solo para administradores.
    /// </summary>
    /// <returns>La vista con la lista de préstamos vencidos.</returns>
    /// 
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Overdue()
    {
        try
        {
            var overdueLoans = await _loanService.GetOverdueLoansAsync();
            return View(overdueLoans);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando préstamos vencidos");
            return View("Error");
        }
    }

    /// <summary>
    /// Cargar la lista de artículos disponibles en el ViewBag.
    /// </summary>
    private async Task LoadItemsForCreate()
    {
        try
        {
            var allItems = await _itemService.GetAllItemsAsync();
            var availableItems = allItems.Where(i => i.Status == ItemStatus.Available).ToList();
            ViewBag.Items = new SelectList(availableItems, "Id", "Name");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando artículos para creación de préstamo");
            ViewBag.Items = new SelectList(new List<ItemDto>(), "Id", "Name");
        }
    }
}