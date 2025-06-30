using MyApp.Business.Dtos.Role;
using MyApp.Business.Dtos.User;

namespace MyApp.Presentation.Models;

/// <summary>
/// ViewModel para la gestión de roles de usuarios
/// </summary>
public class RoleManagementViewModel
{
    public List<UserDto> Users { get; set; } = new List<UserDto>();
    public List<RoleDto> AvailableRoles { get; set; } = new List<RoleDto>();
}

/// <summary>
/// ViewModel para asignación masiva de roles
/// </summary>
public class BulkRoleAssignViewModel
{
    public List<UserDto> Users { get; set; } = new List<UserDto>();
    public List<RoleDto> AvailableRoles { get; set; } = new List<RoleDto>();
    public List<RoleAssignmentItem> RoleAssignments { get; set; } = new List<RoleAssignmentItem>();
}

/// <summary>
/// Representa un item individual para asignación de rol
/// </summary>
public class RoleAssignmentItem
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string CurrentRole { get; set; } = string.Empty;
    public int NewRoleId { get; set; }
    public bool IsSelected { get; set; }
}

/// <summary>
/// ViewModel para cambio individual de rol
/// </summary>
public class ChangeRoleViewModel
{
    [Required(ErrorMessage = "Debe seleccionar un usuario")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un rol")]
    public int NewRoleId { get; set; }

    public string UserName { get; set; } = string.Empty;
    public string CurrentRole { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "El motivo no puede exceder los 200 caracteres")]
    [Display(Name = "Motivo del cambio")]
    public string? Reason { get; set; }
}
