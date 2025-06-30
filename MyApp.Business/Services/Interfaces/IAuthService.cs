namespace MyApp.Business.Services.Interfaces;

/// <summary>
/// Define un contrato para el servicio de autenticación y registro de usuarios.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Autentica a un usuario basándose en sus credenciales.
    /// </summary>
    /// <param name="loginDto">El DTO que contiene el email y la contraseña del usuario.</param>
    /// <returns>Un DTO con los datos del usuario si la autenticación es exitosa; de lo contrario, lanza una excepción o retorna nulo.</returns>
    Task<UserDto> LoginAsync(LoginDto loginDto);

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="registerDto">El DTO que contiene los datos del nuevo usuario.</param>
    /// <returns>Un DTO con los datos del usuario recién creado.</returns>
    Task<UserDto> RegisterAsync(RegisterDto registerDto);

    /// <summary>
    /// Valida si una contraseña proporcionada coincide con la almacenada para un usuario.
    /// </summary>
    /// <param name="email">El email del usuario a validar.</param>
    /// <param name="password">La contraseña en texto plano a comparar.</param>
    /// <returns>True si la contraseña es válida; de lo contrario, false.</returns>
    Task<bool> ValidatePasswordAsync(string email, string password);

    /// <summary>
    /// Crea un hash seguro de una contraseña en texto plano.
    /// </summary>
    /// <param name="password">La contraseña a hashear.</param>
    /// <returns>La representación hash de la contraseña.</returns>
    Task<string> HashPasswordAsync(string password);

    /// <summary>
    /// Realiza las operaciones necesarias para cerrar la sesión de un usuario.
    /// </summary>
    Task LogoutAsync();
}