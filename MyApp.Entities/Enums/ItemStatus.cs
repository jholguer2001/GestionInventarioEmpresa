namespace MyApp.Entities.Enums;

/// <summary>
/// Define los posibles estados en el ciclo de vida de un artículo (Item).
/// </summary>
public enum ItemStatus
{
    /// <summary>
    /// El artículo está en el inventario y disponible para ser prestado.
    /// </summary>
    Available = 0,

    /// <summary>
    /// El artículo ha sido prestado y está actualmente en posesión de un usuario.
    /// </summary>
    OnLoan = 1,

    /// <summary>
    /// El artículo está temporalmente fuera de servicio por mantenimiento o reparación.
    /// </summary>
    Maintenance = 2,

    /// <summary>
    /// El artículo ha sido retirado permanentemente del servicio (ej. por obsoleto o dañado sin reparación).
    /// </summary>
    Decommissioned = 3
}