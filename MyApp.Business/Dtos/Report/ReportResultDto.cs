namespace MyApp.Business.Dtos.Report;

/// <summary>
/// DTO que contiene el resultado de un informe generado, listo para ser descargado.
/// </summary>
public class ReportResultDto
{
    /// <summary>
    /// El nombre sugerido para el archivo del informe (ej. "HistorialDePrestamos.pdf").
    /// </summary>
    [Required]
    public string FileName { get; set; }

    /// <summary>
    /// El contenido binario del archivo del informe.
    /// </summary>
    [Required]
    public byte[] Content { get; set; }

    /// <summary>
    /// El tipo MIME (Content Type) del contenido del archivo (ej. "application/pdf").
    /// </summary>
    [Required]
    public string ContentType { get; set; }
}