namespace MyApp.DataAccess.Context;

/// <summary>
/// Fábrica para crear instancias de <see cref="ApplicationDbContext"/> en tiempo de diseño.
/// Es utilizada por las herramientas de Entity Framework Core (ej. para migraciones).
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <summary>
    /// Crea una nueva instancia del contexto de la base de datos.
    /// </summary>
    /// <param name="args">Argumentos proporcionados por la herramienta de diseño. No se utilizan en esta implementación.</param>
    /// <returns>Una nueva instancia de <see cref="ApplicationDbContext"/>.</returns>
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Construye un objeto de configuración para poder leer el archivo appsettings.json
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Crea un constructor de opciones para el DbContext
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Obtiene la cadena de conexión desde la configuración
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Configura el DbContext para que use SQL Server con la cadena de conexión obtenida
        optionsBuilder.UseSqlServer(connectionString);

        // Retorna una nueva instancia del DbContext con las opciones configuradas
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
