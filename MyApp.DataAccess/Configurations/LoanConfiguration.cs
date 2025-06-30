namespace MyApp.DataAccess.Configurations;

/// <summary>
/// Configura el mapeo de la entidad <see cref="Loan"/> a la base de datos
/// utilizando la API Fluida de Entity Framework Core.
/// </summary>
public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    /// <summary>
    /// Aplica la configuración a la entidad Loan.
    /// </summary>
    /// <param name="builder">El constructor que se utiliza para configurar la entidad.</param>
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        // Define el nombre de la tabla
        builder.ToTable("Loans");

        // --- Clave Primaria ---
        builder.HasKey(l => l.Id);

        // --- Configuración de Propiedades ---
        builder.Property(l => l.RequestDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(l => l.DeliveryDate)
            .IsRequired(false); // Permite nulos

        builder.Property(l => l.ReturnDate)
            .IsRequired(false); // Permite nulos

        // Configura el enum para ser guardado como un entero
        builder.Property(l => l.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(LoanStatus.Pending);

        builder.Property(l => l.Comments)
            .HasMaxLength(500);

        // --- Campos de Auditoría ---
        builder.Property(l => l.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAdd();

        builder.Property(l => l.ModifiedDate)
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnUpdate();

        builder.Property(l => l.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(l => l.ModifiedBy)
            .HasMaxLength(100);

        // --- Índices ---
        // Mejoran el rendimiento de las búsquedas por UserId, ItemId, etc.
        builder.HasIndex(l => l.UserId)
            .HasDatabaseName("IX_Loans_UserId");

        builder.HasIndex(l => l.ItemId)
            .HasDatabaseName("IX_Loans_ItemId");

        builder.HasIndex(l => l.Status)
            .HasDatabaseName("IX_Loans_Status");

        builder.HasIndex(l => l.RequestDate)
            .HasDatabaseName("IX_Loans_RequestDate");

        builder.HasIndex(l => l.CreatedDate)
            .HasDatabaseName("IX_Loans_CreatedDate");

        // --- Relaciones ---
        builder.HasOne(l => l.User)
            .WithMany(u => u.Loans)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict) // Impide borrar un usuario si tiene préstamos
            .HasConstraintName("FK_Loans_Users");

        builder.HasOne(l => l.Item)
            .WithMany(i => i.Loans)
            .HasForeignKey(l => l.ItemId)
            .OnDelete(DeleteBehavior.Restrict) // Impide borrar un item si tiene préstamos
            .HasConstraintName("FK_Loans_Items");
    }
}