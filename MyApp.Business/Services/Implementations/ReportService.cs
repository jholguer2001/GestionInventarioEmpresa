using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ClosedXML.Excel;
using Microsoft.Extensions.Logging;

namespace MyApp.Business.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de negocio para la generación de informes.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditService _auditService;
        private readonly ILogger<ReportService> _logger;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de informes.
        /// </summary>
        /// <param name="unitOfWork">La unidad de trabajo para el acceso a datos.</param>
        /// <param name="auditService">El servicio para registrar acciones de auditoría.</param>
        /// <param name="logger">El servicio de logging para registrar información y errores.</param>
        public ReportService(IUnitOfWork unitOfWork, IAuditService auditService, ILogger<ReportService> logger)
        {
            _unitOfWork = unitOfWork;
            _auditService = auditService;
            _logger = logger;

            // Configura la licencia de QuestPDF al iniciar el servicio.
            try
            {
                QuestPDF.Settings.License = LicenseType.Community;
                _logger.LogInformation("✅ QuestPDF configurado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error configurando QuestPDF");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<byte[]> GenerateTestPdfAsync()
        {
            try
            {
                _logger.LogInformation("🧪 Generando PDF de prueba...");

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header()
                            .Text("PDF DE PRUEBA - SISTEMA FUNCIONANDO")
                            .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(column =>
                            {
                                column.Item().Text($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                                column.Item().PaddingTop(20);
                                column.Item().Text("🎉 ¡QuestPDF está funcionando correctamente!");
                                column.Item().PaddingTop(10);
                                column.Item().Text("Este es un PDF de prueba para verificar que el sistema de reportes funciona.");
                                column.Item().PaddingTop(20);
                                column.Item().Text("Características de prueba:");
                                column.Item().Text("• Generación exitosa de PDF");
                                column.Item().Text("• Soporte de caracteres en español: ñ, á, é, í, ó, ú");
                                column.Item().Text("• Formato y estilos aplicados correctamente");
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text("Sistema de Gestión de Inventario - Prueba exitosa");
                    });
                });

                var pdfBytes = document.GeneratePdf();
                _logger.LogInformation("✅ PDF de prueba generado exitosamente. Tamaño: {Size} bytes", pdfBytes.Length);

                return await Task.FromResult(pdfBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error generando PDF de prueba");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<byte[]> GenerateInventoryStatusReportAsync()
        {
            try
            {
                _logger.LogInformation("🔄 Iniciando generación de reporte de estado de inventario...");

                var items = await _unitOfWork.Items.GetAllAsync();
                var loans = await _unitOfWork.Loans.GetAllAsync();

                _logger.LogInformation("📊 Datos obtenidos: {ItemCount} artículos, {LoanCount} préstamos",
                    items.Count(), loans.Count());

                if (!items.Any())
                {
                    _logger.LogWarning("⚠️ No hay artículos en el sistema");
                    return await GenerateEmptyInventoryReportAsync();
                }

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(11));

                        page.Header()
                            .Text("REPORTE DE ESTADO DEL INVENTARIO")
                            .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(column =>
                            {
                                column.Item().Text($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm:ss}").FontSize(10);
                                column.Item().PaddingTop(20);

                                column.Item().Text("ESTADÍSTICAS GENERALES").SemiBold().FontSize(14).FontColor(Colors.Blue.Medium);
                                column.Item().PaddingTop(10);

                                var totalItems = items.Count();
                                var availableItems = items.Count(i => i.Status == ItemStatus.Available);
                                var onLoanItems = items.Count(i => i.Status == ItemStatus.OnLoan);
                                var maintenanceItems = items.Count(i => i.Status == ItemStatus.Maintenance);
                                var decommissionedItems = items.Count(i => i.Status == ItemStatus.Decommissioned);

                                var totalLoans = loans.Count();
                                var activeLoans = loans.Count(l => l.Status == LoanStatus.Delivered);
                                var pendingLoans = loans.Count(l => l.Status == LoanStatus.Pending);

                                column.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3);
                                        columns.RelativeColumn(1);
                                        columns.RelativeColumn(2);
                                    });

                                    table.Cell().Text("Concepto").SemiBold();
                                    table.Cell().Text("Cantidad").SemiBold();
                                    table.Cell().Text("Porcentaje").SemiBold();
                                    table.Cell().Text("Total de artículos:");
                                    table.Cell().Text(totalItems.ToString());
                                    table.Cell().Text("100%");
                                    table.Cell().Text("Artículos disponibles:");
                                    table.Cell().Text(availableItems.ToString());
                                    table.Cell().Text($"{(totalItems > 0 ? availableItems * 100.0 / totalItems : 0):F1}%");
                                    table.Cell().Text("Artículos en préstamo:");
                                    table.Cell().Text(onLoanItems.ToString());
                                    table.Cell().Text($"{(totalItems > 0 ? onLoanItems * 100.0 / totalItems : 0):F1}%");
                                    table.Cell().Text("En mantenimiento:");
                                    table.Cell().Text(maintenanceItems.ToString());
                                    table.Cell().Text($"{(totalItems > 0 ? maintenanceItems * 100.0 / totalItems : 0):F1}%");
                                    table.Cell().Text("Fuera de servicio:");
                                    table.Cell().Text(decommissionedItems.ToString());
                                    table.Cell().Text($"{(totalItems > 0 ? decommissionedItems * 100.0 / totalItems : 0):F1}%");
                                    table.Cell().Text("Total de préstamos:");
                                    table.Cell().Text(totalLoans.ToString());
                                    table.Cell().Text("-");
                                    table.Cell().Text("Préstamos activos:");
                                    table.Cell().Text(activeLoans.ToString());
                                    table.Cell().Text("-");
                                    table.Cell().Text("Préstamos pendientes:");
                                    table.Cell().Text(pendingLoans.ToString());
                                    table.Cell().Text("-");
                                });

                                column.Item().PaddingTop(30);
                                column.Item().Text("DISTRIBUCIÓN POR CATEGORÍAS").SemiBold().FontSize(14).FontColor(Colors.Blue.Medium);
                                column.Item().PaddingTop(10);

                                var categories = items.GroupBy(i => i.Category).OrderByDescending(g => g.Count()).Take(10);
                                foreach (var category in categories)
                                {
                                    var percentage = (category.Count() * 100.0 / totalItems);
                                    column.Item().Row(row =>
                                    {
                                        row.RelativeItem(3).Text($"{category.Key}:");
                                        row.RelativeItem(1).Text($"{category.Count()}");
                                        row.RelativeItem(1).Text($"({percentage:F1}%)");
                                    });
                                }

                                column.Item().PaddingTop(30);
                                column.Item().Text("RESUMEN DEL SISTEMA").SemiBold().FontSize(14).FontColor(Colors.Green.Medium);
                                column.Item().PaddingTop(10);

                                var utilizationRate = totalItems > 0 ? (onLoanItems * 100.0 / totalItems) : 0;
                                column.Item().Text($"📊 Tasa de utilización: {utilizationRate:F1}%");

                                if (pendingLoans > 0)
                                {
                                    column.Item().Text($"📋 Préstamos pendientes: {pendingLoans}");
                                }

                                column.Item().Text($"🏷️ Categorías diferentes: {items.Select(i => i.Category).Distinct().Count()}");
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Página ");
                                x.CurrentPageNumber();
                                x.Span(" de ");
                                x.TotalPages();
                                x.Span(" - Sistema de Gestión de Inventario");
                            });
                    });
                });

                _logger.LogInformation("🔄 Generando PDF...");
                var pdfBytes = document.GeneratePdf();
                _logger.LogInformation("✅ PDF generado exitosamente. Tamaño: {Size} bytes", pdfBytes.Length);

                await _auditService.LogActionAsync(
                    "Reports", "EXPORT_PDF", "InventoryStatus",
                    null, new { ReportType = "InventoryStatus", ItemCount = items.Count() },
                    "Reporte de estado de inventario exportado a PDF");

                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error detallado generando reporte de inventario: {Message}", ex.Message);
                _logger.LogError("❌ Inner exception: {InnerException}", ex.InnerException?.Message);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<byte[]> GenerateItemsReportPdfAsync()
        {
            try
            {
                _logger.LogInformation("🔄 Generando reporte de todos los artículos...");
                var items = await _unitOfWork.Items.GetAllAsync();

                if (!items.Any())
                {
                    return await GenerateEmptyInventoryReportAsync();
                }

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.DefaultTextStyle(x => x.FontSize(10));

                        page.Header()
                            .Text("LISTADO COMPLETO DE ARTÍCULOS")
                            .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);

                        page.Content()
                            .Column(column =>
                            {
                                column.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm:ss} | Total: {items.Count()} artículos");
                                column.Item().PaddingTop(15);

                                column.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(2); // Código
                                        columns.RelativeColumn(4); // Nombre
                                        columns.RelativeColumn(2); // Categoría
                                        columns.RelativeColumn(2); // Estado
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Element(HeaderStyle).Text("Código").SemiBold();
                                        header.Cell().Element(HeaderStyle).Text("Nombre").SemiBold();
                                        header.Cell().Element(HeaderStyle).Text("Categoría").SemiBold();
                                        header.Cell().Element(HeaderStyle).Text("Estado").SemiBold();
                                    });

                                    foreach (var item in items.OrderBy(i => i.Code))
                                    {
                                        table.Cell().Element(CellStyle).Text(item.Code ?? "N/A");
                                        table.Cell().Element(CellStyle).Text(item.Name ?? "Sin nombre");
                                        table.Cell().Element(CellStyle).Text(item.Category ?? "Sin categoría");
                                        table.Cell().Element(CellStyle).Text(GetStatusText(item.Status));
                                    }

                                    static IContainer HeaderStyle(IContainer c) => c.Border(1).BorderColor(Colors.Grey.Medium).Background(Colors.Grey.Lighten3).Padding(5);
                                    static IContainer CellStyle(IContainer c) => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5);
                                });
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Página ");
                                x.CurrentPageNumber();
                                x.Span(" de ");
                                x.TotalPages();
                                x.Span(" - Sistema de Gestión de Inventario");
                            });
                    });
                });

                var pdfBytes = document.GeneratePdf();
                _logger.LogInformation("✅ PDF con todos los artículos generado. Total: {Count} productos", items.Count());

                await _auditService.LogActionAsync("Reports", "EXPORT_PDF", "AllItems", null,
                    new { ItemCount = items.Count() }, "Listado completo de artículos exportado");

                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error generando listado de artículos");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<byte[]> GenerateLoansReportExcelAsync()
        {
            var loans = await _unitOfWork.Loans.GetAllAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Préstamos");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Usuario";
            worksheet.Cell(1, 3).Value = "Artículo";
            worksheet.Cell(1, 4).Value = "Estado";

            int row = 2;
            foreach (var loan in loans.Take(1000))
            {
                worksheet.Cell(row, 1).Value = loan.Id;
                worksheet.Cell(row, 2).Value = loan.User?.Name ?? "Desconocido";
                worksheet.Cell(row, 3).Value = loan.Item?.Name ?? "Desconocido";
                worksheet.Cell(row, 4).Value = loan.Status.ToString();
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        /// <inheritdoc/>
        public async Task<byte[]> GenerateUserActivityReportAsync(DateTime fromDate, DateTime toDate)
        {
            var logs = await _unitOfWork.AuditLogs.GetByDateRangeAsync(fromDate, toDate);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Actividad");

            worksheet.Cell(1, 1).Value = "Fecha";
            worksheet.Cell(1, 2).Value = "Usuario";
            worksheet.Cell(1, 3).Value = "Acción";

            int row = 2;
            foreach (var log in logs.Take(1000))
            {
                worksheet.Cell(row, 1).Value = log.ActionDate;
                worksheet.Cell(row, 2).Value = log.ActionBy;
                worksheet.Cell(row, 3).Value = log.Action;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        /// <summary>
        /// Genera un PDF indicando que no hay datos de inventario para reportar.
        /// </summary>
        /// <returns>Un Task que resulta en un arreglo de bytes que representa el PDF vacío.</returns>
        private Task<byte[]> GenerateEmptyInventoryReportAsync()
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);

                    page.Header()
                        .Text("REPORTE DE ESTADO DEL INVENTARIO")
                        .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Item().Text($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                            column.Item().PaddingTop(50);
                            column.Item().Text("⚠️ NO HAY DATOS DISPONIBLES").SemiBold().FontSize(16).FontColor(Colors.Orange.Medium);
                            column.Item().PaddingTop(20);
                            column.Item().Text("No se encontraron artículos en el sistema para generar el reporte.");
                            column.Item().Text("Por favor, agregue artículos al inventario e intente nuevamente.");
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text("Sistema de Gestión de Inventario");
                });
            });

            return Task.FromResult(document.GeneratePdf());
        }

        /// <summary>
        /// Convierte un valor de ItemStatus a un texto legible en español.
        /// </summary>
        /// <param name="status">El valor del enumerador ItemStatus.</param>
        /// <returns>La representación en texto y en español del estado.</returns>
        private static string GetStatusText(ItemStatus status)
        {
            return status switch
            {
                ItemStatus.Available => "Disponible",
                ItemStatus.OnLoan => "En Préstamo",
                ItemStatus.Maintenance => "Mantenimiento",
                ItemStatus.Decommissioned => "Fuera de Servicio",
                _ => "Desconocido"
            };
        }
    }
}
