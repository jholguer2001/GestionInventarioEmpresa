
using QuestPDF.Infrastructure;
using Serilog;

namespace MyApp.Presentation;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // ✅ CONFIGURAR QUESTPDF LICENCIA
        QuestPDF.Settings.License = LicenseType.Community;

        // ✅ SERILOG
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/inventory-.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Host.UseSerilog();

        // ✅ Servicios
        builder.Services.AddControllersWithViews();
        builder.Services.AddApplicationServices(builder.Configuration);

        var app = builder.Build();

        // ✅ Pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // ✅ Inicializar base de datos
        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                Log.Information("✅ Base de datos inicializada correctamente");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "❌ Error al inicializar la base de datos");
                throw;
            }
        }

        Log.Information("🚀 Sistema de Inventario iniciando...");
        app.Run();
    }
}
