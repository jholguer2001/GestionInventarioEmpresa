namespace MyApp.DataAccess.Configurations;

/// <summary>
/// Configura el mapeo de la entidad <see cref="User"/> a la base de datos
/// utilizando la API Fluida de Entity Framework Core.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Aplica la configuración a la entidad User.
    /// </summary>
    /// <param name="builder">El constructor que se utiliza para configurar la entidad.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Define el nombre de la tabla
        builder.ToTable("Users");

        // --- Clave Primaria ---
        builder.HasKey(u => u.Id);

        // --- Configuración de Propiedades ---
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // --- Campos de Auditoría ---
        builder.Property(u => u.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.ModifiedDate)
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnUpdate();

        builder.Property(u => u.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(u => u.ModifiedBy)
            .HasMaxLength(100);

        // --- Índices ---
        builder.HasIndex(u => u.Email)
            .IsUnique() // Asegura que el email sea único en la tabla
            .HasDatabaseName("IX_Users_Email");

        builder.HasIndex(u => u.RoleId)
            .HasDatabaseName("IX_Users_RoleId");

        builder.HasIndex(u => u.CreatedDate)
            .HasDatabaseName("IX_Users_CreatedDate");

        // --- Relaciones ---
        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict) // Impide borrar un rol si tiene usuarios
            .HasConstraintName("FK_Users_Roles");

        builder.HasMany(u => u.Loans)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Impide borrar un usuario si tiene préstamos
    }
}