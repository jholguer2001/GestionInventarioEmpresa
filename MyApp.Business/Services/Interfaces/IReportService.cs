namespace MyApp.Business.Services.Interfaces;

/// <summary>
/// Define un contrato para el servicio de negocio que maneja la generación de informes.
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Genera un informe en formato PDF con el listado de todos los artículos del inventario.
    /// </summary>
    /// <returns>Un arreglo de bytes que representa el archivo PDF.</returns>
    Task<byte[]> GenerateItemsReportPdfAsync();

    /// <summary>
    /// Genera un informe en formato Excel con el historial de todos los préstamos.
    /// </summary>
    /// <returns>Un arreglo de bytes que representa el archivo Excel.</returns>
    Task<byte[]> GenerateLoansReportExcelAsync();

    /// <summary>
    /// Genera un informe de la actividad de todos los usuarios en un rango de fechas específico.
    /// </summary>
    /// <param name="fromDate">La fecha de inicio del período del informe.</param>
    /// <param name="toDate">La fecha de fin del período del informe.</param>
    /// <returns>Un arreglo de bytes que representa el archivo del informe (ej. PDF o CSV).</returns>
    Task<byte[]> GenerateUserActivityReportAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Genera un informe que resume el estado actual del inventario (ej. cuántos artículos están disponibles, prestados, etc.).
    /// </summary>
    /// <returns>Un arreglo de bytes que representa el archivo del informe.</returns>
    Task<byte[]> GenerateInventoryStatusReportAsync();

    /// <summary>
    /// Genera un archivo PDF simple para fines de prueba y depuración.
    /// </summary>
    /// <returns>Un arreglo de bytes que representa un archivo PDF de prueba.</returns>
    Task<byte[]> GenerateTestPdfAsync();
}
