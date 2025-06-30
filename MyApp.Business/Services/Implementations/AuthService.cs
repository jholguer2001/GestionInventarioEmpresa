namespace MyApp.Business.Services.Implementations;

/// <summary>
/// Implementación del servicio de autenticación y registro de usuarios.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuditService _auditService;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de autenticación.
    /// </summary>
    /// <param name="unitOfWork">La unidad de trabajo para el acceso a datos.</param>
    /// <param name="httpContextAccessor">El accesor para el HttpContext actual.</param>
    /// <param name="auditService">El servicio para registrar acciones de auditoría.</param>
    public AuthService(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor,
        IAuditService auditService)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _auditService = auditService;
    }

    /// <inheritdoc/>
    public async Task<UserDto> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);

            if (user == null || !user.IsActive)
            {
                return null; // Usuario no encontrado o inactivo
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return null; // Contraseña incorrecta
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = loginDto.RememberMe,
                ExpiresUtc = loginDto.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(8)
            };

            // Inicia la sesión del usuario creando la cookie de autenticación
            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Intenta registrar la auditoría, pero no bloquea el login si falla
            try
            {
                await _auditService.LogActionAsync(
                    "Users", "LOGIN", user.Id.ToString(),
                    new { },
                    new { Email = user.Email, LoginTime = DateTime.UtcNow },
                    "Successful login");
            }
            catch (Exception auditEx)
            {
                Console.WriteLine($"Audit error during login: {auditEx.Message}");
            }

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
        catch (Exception ex)
        {
            Console.WriteLine($"Login error for {loginDto.Email}: {ex.Message}");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            if (await _unitOfWork.Users.EmailExistsAsync(registerDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var defaultRole = await _unitOfWork.Roles.GetByNameAsync("Operator");
            if (defaultRole == null)
            {
                throw new InvalidOperationException("Default role 'Operator' not found");
            }

            var passwordHash = await HashPasswordAsync(registerDto.Password);

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                RoleId = defaultRole.Id,
                IsActive = true,
                CreatedBy = "System",
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            user = await _unitOfWork.Users.GetByIdAsync(user.Id);

            try
            {
                await _auditService.LogActionAsync(
                    "Users", "REGISTER", user.Id.ToString(),
                    new { },
                    new { Email = user.Email, Role = user.Role.Name },
                    "User registration");
            }
            catch (Exception auditEx)
            {
                Console.WriteLine($"Audit error during registration: {auditEx.Message}");
            }

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
        catch (Exception ex)
        {
            Console.WriteLine($"Registration error for {registerDto.Email}: {ex.Message}");
            throw; // Relanza la excepción para que sea manejada por una capa superior
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ValidatePasswordAsync(string email, string password)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null || !user.IsActive)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Password validation error: {ex.Message}");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<string> HashPasswordAsync(string password)
    {
        // BCrypt es una operación síncrona, se envuelve en un Task para mantener la firma asíncrona.
        return await Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
    }

    /// <inheritdoc/>
    public async Task LogoutAsync()
    {
        try
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;
            var userEmail = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            var userId = currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Cierra la sesión del usuario eliminando la cookie de autenticación
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Intenta registrar la auditoría después del logout
            if (!string.IsNullOrEmpty(userEmail))
            {
                try
                {
                    await _auditService.LogActionAsync(
                        "Users", "LOGOUT", userId ?? "Unknown",
                        new { },
                        new { Email = userEmail, LogoutTime = DateTime.UtcNow },
                        "User logout");
                }
                catch (Exception auditEx)
                {
                    Console.WriteLine($"Audit error during logout: {auditEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logout error: {ex.Message}");
        }
    }
}