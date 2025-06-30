namespace MyApp.Presentation.Controllers;

/// <summary>
/// Gestiona las acciones de cuenta de usuario como login, logout y registro.
/// </summary>
public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AccountController> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de cuentas.
    /// </summary>
    /// <param name="authService">El servicio para la lógica de autenticación.</param>
    /// <param name="logger">El servicio para registrar logs.</param>
    public AccountController(IAuthService authService, ILogger<AccountController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Muestra la vista de inicio de sesión.
    /// </summary>
    /// <param name="returnUrl">La URL a la que se redirigirá al usuario después de un inicio de sesión exitoso.</param>
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity is { IsAuthenticated: true })
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    /// <summary>
    /// Procesa la solicitud de inicio de sesión de un usuario.
    /// </summary>
    /// <param name="model">El modelo con las credenciales del usuario.</param>
    /// <param name="returnUrl">La URL de retorno opcional.</param>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var loginDto = new LoginDto
            {
                Email = model.Email,
                Password = model.Password,
                RememberMe = model.RememberMe
            };

            var user = await _authService.LoginAsync(loginDto);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
                return View(model);
            }

            _logger.LogInformation("Usuario {Email} inició sesión exitosamente", user.Email);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el intento de inicio de sesión para {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "Ocurrió un error durante el inicio de sesión. Por favor, inténtalo de nuevo.");
            return View(model);
        }
    }

    /// <summary>
    /// Muestra la vista de registro de nuevo usuario.
    /// </summary>
    [AllowAnonymous]
    public IActionResult Register()
    {
        if (User.Identity is { IsAuthenticated: true })
            return RedirectToAction("Index", "Home");

        return View();
    }

    /// <summary>
    /// Procesa la solicitud de registro de un nuevo usuario.
    /// </summary>
    /// <param name="model">El modelo con los datos del nuevo usuario.</param>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var registerDto = new RegisterDto
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword
            };

            var user = await _authService.RegisterAsync(registerDto);

            if (user != null)
            {
                _logger.LogInformation("Nuevo usuario registrado: {Email}", user.Email);
                TempData["SuccessMessage"] = "¡Registro exitoso! Ahora puedes iniciar sesión.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "El registro falló. Por favor, inténtalo de nuevo.");
            return View(model);
        }
        catch (InvalidOperationException ex) // Captura errores de negocio específicos
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (Exception ex) // Captura errores inesperados
        {
            _logger.LogError(ex, "Error durante el registro para {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "Ocurrió un error durante el registro. Por favor, inténtalo de nuevo.");
            return View(model);
        }
    }

    /// <summary>
    /// Cierra la sesión del usuario actualmente autenticado.
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        _logger.LogInformation("Usuario cerró sesión");
        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Muestra la página de acceso denegado.
    /// </summary>
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}