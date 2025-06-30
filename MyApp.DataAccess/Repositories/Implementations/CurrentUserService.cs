
namespace MyApp.DataAccess.Repositories.Implementations;

/// <summary>
/// Implementación de <see cref="ICurrentUserService"/> que obtiene la información
/// del usuario actual a partir del HttpContext.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Inicializa una nueva instancia del servicio.
    /// </summary>
    /// <param name="httpContextAccessor">El accesor para obtener el HttpContext actual.</param>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    public string? GetCurrentUserEmail()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context?.User?.Identity?.IsAuthenticated == true)
        {
            return context.User.FindFirst(ClaimTypes.Email)?.Value;
        }
        // Devuelve un valor predeterminado si no hay usuario autenticado
        return "system@inventory.com";
    }

    /// <inheritdoc/>
    public string? GetCurrentUserName()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context?.User?.Identity?.IsAuthenticated == true)
        {
            return context.User.FindFirst(ClaimTypes.Name)?.Value;
        }
        return "System";
    }

    /// <inheritdoc/>
    public int? GetCurrentUserId()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context?.User?.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
        }
        return null;
    }

    /// <inheritdoc/>
    public string? GetCurrentUserRole()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context?.User?.Identity?.IsAuthenticated == true)
        {
            return context.User.FindFirst(ClaimTypes.Role)?.Value;
        }
        return "System";
    }

    /// <inheritdoc/>
    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
    }

    /// <inheritdoc/>
    public string? GetClientIpAddress()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            // Primero, intenta obtener la IP de la cabecera 'X-Forwarded-For',
            // que es común cuando la aplicación está detrás de un proxy.
            var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwarded))
            {
                // La cabecera puede contener una lista de IPs, la primera es la del cliente original.
                return forwarded.Split(',').FirstOrDefault()?.Trim();
            }

            // Otra cabecera común para la IP real detrás de un proxy.
            var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp))
            {
                return realIp;
            }

            // Si no hay cabeceras de proxy, usa la IP de la conexión directa.
            return context.Connection.RemoteIpAddress?.ToString();
        }
        return "Unknown";
    }

    /// <inheritdoc/>
    public string? GetUserAgent()
    {
        var context = _httpContextAccessor.HttpContext;
        return context?.Request.Headers["User-Agent"].FirstOrDefault();
    }
}