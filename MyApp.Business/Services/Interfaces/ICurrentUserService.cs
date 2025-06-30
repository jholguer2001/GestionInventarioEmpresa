namespace MyApp.Business.Services.Interfaces;

/// <summary>
/// Define un contrato para un servicio que proporciona información sobre el usuario actual.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Obtiene el correo electrónico del usuario actualmente autenticado.
    /// </summary>
    /// <returns>El email del usuario, o nulo si no está disponible.</returns>
    string? GetCurrentUserEmail();

    /// <summary>
    /// Obtiene el nombre del usuario actualmente autenticado.
    /// </summary>
    /// <returns>El nombre del usuario, o nulo si no está disponible.</returns>
    string? GetCurrentUserName();

    /// <summary>
    /// Obtiene el ID único del usuario actualmente autenticado.
    /// </summary>
    /// <returns>El ID del usuario, o nulo si no está autenticado.</returns>
    int? GetCurrentUserId();

    /// <summary>
    /// Obtiene el rol o roles del usuario actualmente autenticado.
    /// </summary>
    /// <returns>El nombre del rol, o nulo si no está disponible.</returns>
    string? GetCurrentUserRole();

    /// <summary>
    /// Verifica si el usuario actual está autenticado.
    /// </summary>
    /// <returns>True si el usuario está autenticado; de lo contrario, false.</returns>
    bool IsAuthenticated();

    /// <summary>
    /// Obtiene la dirección IP del cliente que realiza la solicitud.
    /// </summary>
    /// <returns>La dirección IP del cliente, o nulo si no se puede determinar.</returns>
    string? GetClientIpAddress();

    /// <summary>
    /// Obtiene la cadena User-Agent del cliente que realiza la solicitud.
    /// </summary>
    /// <returns>La cadena User-Agent, o nulo si no está disponible.</returns>
    string? GetUserAgent();
}