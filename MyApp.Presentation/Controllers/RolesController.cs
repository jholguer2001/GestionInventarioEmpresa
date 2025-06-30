using MyApp.Business.Dtos.Role;
using MyApp.Business.Dtos.User;
namespace MyApp.Presentation.Controllers;

/// <summary>
/// Controlador simplificado para cambiar roles de usuarios.
/// Cumple con RF1.3: Solo usuarios "Administrador" pueden asignar roles.
/// </summary>
[Authorize(Roles = "Administrator")]
public class RolesController : Controller
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RolesController> _logger;

    public RolesController(IUserService userService, IUnitOfWork unitOfWork, ILogger<RolesController> logger)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Muestra la lista de usuarios para cambiar sus roles
    /// </summary>
    public async Task<IActionResult> Index()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            var roles = await _unitOfWork.Roles.GetAllAsync();

            ViewBag.AvailableRoles = roles.ToList();
            return View(users.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando gestión de roles");
            TempData["ErrorMessage"] = "Error cargando la gestión de roles.";
            return View(new List<UserDto>());
        }
    }

    /// <summary>
    /// Cambia el rol de un usuario específico
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeRole(int userId, int newRoleId)
    {
        try
        {
            // Obtener información del usuario y rol
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Usuario no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var role = await _unitOfWork.Roles.GetByIdAsync(newRoleId);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Rol no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            // Protección: No permitir que el admin se quite su propio rol de administrador
            var currentUserId = GetCurrentUserId();
            if (userId == currentUserId && role.Name != "Administrator")
            {
                TempData["ErrorMessage"] = "No puedes cambiar tu propio rol de Administrador.";
                return RedirectToAction(nameof(Index));
            }

            // Cambiar el rol
            var success = await _userService.ChangeUserRoleAsync(userId, newRoleId);

            if (success)
            {
                _logger.LogInformation("Rol del usuario {UserId} ({UserName}) cambiado a {RoleName} por administrador {AdminId}",
                    userId, user.Name, role.Name, currentUserId);
                TempData["SuccessMessage"] = $"Rol de {user.Name} cambiado exitosamente a {role.Name}.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al cambiar el rol del usuario.";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cambiando rol del usuario {UserId} a rol {RoleId}", userId, newRoleId);
            TempData["ErrorMessage"] = "Error al cambiar el rol del usuario.";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Obtiene el ID del usuario actual desde los claims
    /// </summary>
    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out int userId) ? userId : 0;
    }
}