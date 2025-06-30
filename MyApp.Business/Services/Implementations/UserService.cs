
namespace MyApp.Business.Services.Implementations;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditService _auditService;

    public UserService(IUnitOfWork unitOfWork, IAuditService auditService)
    {
        _unitOfWork = unitOfWork;
        _auditService = auditService;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            RoleName = u.Role.Name,
            IsActive = u.IsActive,
            CreatedDate = u.CreatedDate
        });
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            RoleName = user.Role.Name,
            IsActive = user.IsActive,
            CreatedDate = user.CreatedDate
        };
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // Validar email único
        if (await _unitOfWork.Users.EmailExistsAsync(createUserDto.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        // Validar que el rol existe
        var role = await _unitOfWork.Roles.GetByIdAsync(createUserDto.RoleId);
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {createUserDto.RoleId} not found");
        }

        // Hashear contraseña
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

        var user = new User
        {
            Name = createUserDto.Name,
            Email = createUserDto.Email,
            PasswordHash = passwordHash,
            RoleId = createUserDto.RoleId,
            IsActive = true
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Recargar usuario con rol
        user = await _unitOfWork.Users.GetByIdAsync(user.Id);

        // Auditoría
        await _auditService.LogActionAsync(
            "Users", "CREATE", user.Id.ToString(),
            null, new { user.Name, user.Email, Role = user.Role.Name },
            "User created by admin");

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            RoleName = user.Role.Name,
            IsActive = user.IsActive,
            CreatedDate = user.CreatedDate
        };
    }

    public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        // Guardar valores anteriores para auditoría
        var oldValues = new { user.Name, user.Email, RoleId = user.RoleId, user.IsActive };

        // Validar email único (excluyendo el usuario actual)
        var emailExists = await _unitOfWork.Users.FindAsync(u =>
            u.Email.ToLower() == updateUserDto.Email.ToLower() && u.Id != id);

        if (emailExists.Any())
        {
            throw new InvalidOperationException("Email already exists");
        }

        // Validar que el rol existe
        var role = await _unitOfWork.Roles.GetByIdAsync(updateUserDto.RoleId);
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {updateUserDto.RoleId} not found");
        }

        // Actualizar propiedades
        user.Name = updateUserDto.Name;
        user.Email = updateUserDto.Email;
        user.RoleId = updateUserDto.RoleId;
        user.IsActive = updateUserDto.IsActive;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Recargar usuario con rol
        user = await _unitOfWork.Users.GetByIdAsync(user.Id);

        // Auditoría
        var newValues = new { user.Name, user.Email, RoleId = user.RoleId, user.IsActive };
        await _auditService.LogActionAsync(
            "Users", "UPDATE", user.Id.ToString(),
            oldValues, newValues, "User updated by admin");

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            RoleName = user.Role.Name,
            IsActive = user.IsActive,
            CreatedDate = user.CreatedDate
        };
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            return false;

        // Verificar si el usuario tiene préstamos activos
        var activeLoans = await _unitOfWork.Loans.GetByUserAsync(id);
        if (activeLoans.Any(l => l.Status == Entities.Enums.LoanStatus.Pending ||
                               l.Status == Entities.Enums.LoanStatus.Approved ||
                               l.Status == Entities.Enums.LoanStatus.Delivered))
        {
            throw new InvalidOperationException("Cannot delete user with active loans");
        }

        var oldValues = new { user.Name, user.Email, user.IsActive };

        await _unitOfWork.Users.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        // Auditoría
        await _auditService.LogActionAsync(
            "Users", "DELETE", id.ToString(),
            oldValues, null, "User deleted by admin");

        return true;
    }

    public async Task<bool> ChangeUserRoleAsync(int userId, int roleId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return false;

        var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
        if (role == null)
            return false;

        var oldRole = user.Role.Name;
        user.RoleId = roleId;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Auditoría
        await _auditService.LogActionAsync(
            "Users", "ROLE_CHANGE", userId.ToString(),
            new { OldRole = oldRole },
            new { NewRole = role.Name },
            $"Role changed from {oldRole} to {role.Name}");

        return true;
    }

    public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId)
    {
        var users = await _unitOfWork.Users.GetByRoleAsync(roleId);
        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            RoleName = u.Role.Name,
            IsActive = u.IsActive,
            CreatedDate = u.CreatedDate
        });
    }
}
