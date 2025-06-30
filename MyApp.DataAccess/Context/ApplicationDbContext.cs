namespace MyApp.DataAccess.Context;

/// <summary>
/// Representa la sesión con la base de datos y permite consultar y guardar entidades.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Inicializa una nueva instancia del contexto de la aplicación.
    /// </summary>
    /// <param name="options">Las opciones para configurar el contexto.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // --- DbSet para cada entidad del modelo ---
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    /// <summary>
    /// Configura el modelo de datos aplicando las configuraciones de las entidades.
    /// </summary>
    /// <param name="modelBuilder">El constructor utilizado para crear el modelo.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas las configuraciones definidas en el ensamblado actual
        // que implementan IEntityTypeConfiguration<T>.
        // Nota: Una forma más limpia sería modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new ItemConfiguration());
        modelBuilder.ApplyConfiguration(new LoanConfiguration());
        modelBuilder.ApplyConfiguration(new AuditLogConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
