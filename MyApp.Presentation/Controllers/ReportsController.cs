namespace MyApp.Presentation.Controllers;

[Authorize(Roles = "Administrator")]
public class ReportsController : Controller
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ExportItemsPdf()
    {
        try
        {
            _logger.LogInformation("🔄 Iniciando generación de reporte PDF de artículos...");

            var pdfBytes = await _reportService.GenerateItemsReportPdfAsync();

            _logger.LogInformation("✅ Reporte PDF de artículos generado exitosamente. Tamaño: {Size} bytes", pdfBytes.Length);

            return File(pdfBytes, "application/pdf", $"Reporte_Articulos_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error generando reporte PDF de artículos");
            TempData["ErrorMessage"] = $"Error generando reporte PDF: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> ExportInventoryStatusPdf()
    {
        try
        {
            _logger.LogInformation("🔄 Iniciando generación de reporte PDF de estado de inventario...");

            // ✅ AGREGAR TIMEOUT Y VALIDACIONES
            var cts = new CancellationTokenSource(TimeSpan.FromMinutes(2));

            var pdfBytes = await _reportService.GenerateInventoryStatusReportAsync();

            if (pdfBytes == null || pdfBytes.Length == 0)
            {
                throw new InvalidOperationException("El PDF generado está vacío");
            }

            _logger.LogInformation("✅ Reporte PDF de estado de inventario generado exitosamente. Tamaño: {Size} bytes", pdfBytes.Length);

            return File(pdfBytes, "application/pdf", $"Estado_Inventario_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error generando reporte PDF de estado de inventario: {Message}", ex.Message);
            _logger.LogError("❌ Stack trace: {StackTrace}", ex.StackTrace);

            TempData["ErrorMessage"] = $"Error generando reporte de estado de inventario: {ex.Message}. Revisa los logs para más detalles.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> ExportLoansExcel()
    {
        try
        {
            _logger.LogInformation("🔄 Iniciando generación de reporte Excel de préstamos...");

            var excelBytes = await _reportService.GenerateLoansReportExcelAsync();

            _logger.LogInformation("✅ Reporte Excel de préstamos generado exitosamente");

            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                       $"Reporte_Prestamos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error generando reporte Excel de préstamos");
            TempData["ErrorMessage"] = $"Error generando reporte Excel: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> ExportUserActivity(DateTime? fromDate, DateTime? toDate)
    {
        try
        {
            var from = fromDate ?? DateTime.Now.AddDays(-30);
            var to = toDate ?? DateTime.Now;

            _logger.LogInformation("🔄 Iniciando generación de reporte de actividad de usuario para período {From} a {To}", from, to);

            var excelBytes = await _reportService.GenerateUserActivityReportAsync(from, to);

            _logger.LogInformation("✅ Reporte Excel de actividad de usuario generado exitosamente");

            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                       $"Actividad_Usuario_{from:yyyyMMdd}_a_{to:yyyyMMdd}.xlsx");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error generando reporte Excel de actividad de usuario");
            TempData["ErrorMessage"] = $"Error generando reporte de actividad: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    // 🎯 MÉTODO DE PRUEBA PARA DEBUGGING
    [HttpPost]
    public async Task<IActionResult> TestPdfGeneration()
    {
        try
        {
            _logger.LogInformation("🧪 Iniciando prueba de generación de PDF...");

            // PDF de prueba simple
            var testBytes = await _reportService.GenerateTestPdfAsync();

            _logger.LogInformation("✅ PDF de prueba generado exitosamente");

            return File(testBytes, "application/pdf", "Test_PDF.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error en prueba de PDF");
            TempData["ErrorMessage"] = $"Error en prueba: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}