namespace MyApp.DataAccess.Configurations;

/// <summary>
/// Configura el mapeo de la entidad <see cref="Item"/> a la base de datos
/// utilizando la API Fluida de Entity Framework Core.
/// </summary>
public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    /// <summary>
    /// Aplica la configuración a la entidad Item.
    /// </summary>
    /// <param name="builder">El constructor que se utiliza para configurar la entidad.</param>
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        // Define el nombre de la tabla
        builder.ToTable("Items");

        // --- Clave Primaria ---
        builder.HasKey(i => i.Id);

        // --- Configuración de Propiedades ---
        builder.Property(i => i.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Category)
            .IsRequired()
            .HasMaxLength(50);

        // Configura el enum para ser guardado como un entero en la BD
        builder.Property(i => i.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(ItemStatus.Available);

        builder.Property(i => i.Location)
            .HasMaxLength(100);

        // --- Campos de Auditoría ---
        builder.Property(i => i.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()") // Valor por defecto en la BD
            .ValueGeneratedOnAdd(); // EF sabe que es generado al añadir

        builder.Property(i => i.ModifiedDate)
            .HasDefaultValueSql("GETDATE()") // Valor por defecto en la BD
            .ValueGeneratedOnUpdate(); // EF sabe que es generado al actualizar

        builder.Property(i => i.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(i => i.ModifiedBy)
            .HasMaxLength(100);

        // --- Índices ---
        builder.HasIndex(i => i.Code)
            .IsUnique() // Asegura que el código del item sea único
            .HasDatabaseName("IX_Items_Code");

        builder.HasIndex(i => i.Category)
            .HasDatabaseName("IX_Items_Category");

        builder.HasIndex(i => i.Status)
            .HasDatabaseName("IX_Items_Status");

        builder.HasIndex(i => i.Name)
            .HasDatabaseName("IX_Items_Name");

        builder.HasIndex(i => i.CreatedDate)
            .HasDatabaseName("IX_Items_CreatedDate");

        // --- Relaciones ---
        builder.HasMany(i => i.Loans)
            .WithOne(l => l.Item)
            .HasForeignKey(l => l.ItemId)
            .OnDelete(DeleteBehavior.Restrict); // Evita borrar un item si tiene préstamos
    }
}