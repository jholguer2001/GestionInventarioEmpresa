namespace MyApp.Business.Dtos.Report;

/// <summary>
/// DTO que contiene los parámetros para una solicitud de generación de informes.
/// </summary>
public class ReportRequestDto
{
    /// <summary>
    /// La fecha de inicio opcional para el rango del informe.
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// La fecha de fin opcional para el rango del informe.
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// El tipo de informe a generar (ej. "LoanHistory", "ItemInventory").
    /// </summary>
    [Required(ErrorMessage = "El tipo de informe es obligatorio.")]
    public string ReportType { get; set; }

    /// <summary>
    /// Un arreglo opcional de categorías para filtrar el informe.
    /// </summary>
    public string[]? Categories { get; set; }

    /// <summary>
    /// Un arreglo opcional de IDs de usuario para filtrar el informe.
    /// </summary>
    public int[]? UserIds { get; set; }
}