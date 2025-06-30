namespace MyApp.DataAccess.Configurations;

/// <summary>
/// Configura el mapeo de la entidad <see cref="Role"/> a la base de datos
/// y siembra los datos iniciales de roles.
/// </summary>
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    /// <summary>
    /// Aplica la configuración a la entidad Role.
    /// </summary>
    /// <param name="builder">El constructor que se utiliza para configurar la entidad.</param>
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Define el nombre de la tabla
        builder.ToTable("Roles");

        // --- Clave Primaria ---
        builder.HasKey(r => r.Id);

        // --- Configuración de Propiedades ---
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Description)
            .HasMaxLength(200);

        // --- Campos de Auditoría ---
        builder.Property(r => r.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.ModifiedDate)
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnUpdate();

        builder.Property(r => r.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(r => r.ModifiedBy)
            .HasMaxLength(100);

        // --- Índices ---
        builder.HasIndex(r => r.Name)
            .IsUnique() // Asegura que no haya dos roles con el mismo nombre
            .HasDatabaseName("IX_Roles_Name");

        // --- Relaciones ---
        builder.HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict); // Impide borrar un rol si tiene usuarios asignados

        // --- Siembra de Datos (Seed) ---
        builder.HasData(
            new Role { Id = 1, Name = "Administrator", Description = "Full system access", CreatedDate = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc), CreatedBy = "System" }, // <-- ¡CORREGIDO AQUÍ!
            new Role { Id = 2, Name = "Operator", Description = "Limited system access", CreatedDate = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc), CreatedBy = "System" }  // <-- ¡Y AQUÍ!
        );
    }
}