/// <summary>
/// Configura el mapeo de la entidad <see cref="AuditLog"/> a la base de datos
/// utilizando la API Fluida de Entity Framework Core.
/// </summary>
public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    /// <summary>
    /// Aplica la configuración a la entidad AuditLog.
    /// </summary>
    /// <param name="builder">El constructor que se utiliza para configurar la entidad.</param>
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        // Define el nombre de la tabla en la base de datos
        builder.ToTable("AuditLogs");

        // --- Clave Primaria ---
        builder.HasKey(a => a.Id);

        // --- Configuración de Propiedades ---
        builder.Property(a => a.TableName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Action)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.PrimaryKey)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.OldValues)
            .HasColumnType("nvarchar(max)")
            .IsRequired(false); // Permite valores nulos

        builder.Property(a => a.NewValues)
            .HasColumnType("nvarchar(max)")
            .IsRequired(false); // Permite valores nulos

        builder.Property(a => a.ActionDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()"); // Valor por defecto a nivel de BD

        builder.Property(a => a.ActionBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ActionDescription)
            .HasMaxLength(200)
            .IsRequired(false); // Permite valores nulos

        builder.Property(a => a.IpAddress)
            .HasMaxLength(45)
            .IsRequired(false); // Permite valores nulos

        builder.Property(a => a.UserAgent)
            .HasMaxLength(500)
            .IsRequired(false); // Permite valores nulos

        // --- Índices ---
        // Mejoran el rendimiento de las consultas sobre estas columnas
        builder.HasIndex(a => a.TableName)
            .HasDatabaseName("IX_AuditLogs_TableName");

        builder.HasIndex(a => a.PrimaryKey)
            .HasDatabaseName("IX_AuditLogs_PrimaryKey");

        builder.HasIndex(a => a.ActionDate)
            .HasDatabaseName("IX_AuditLogs_ActionDate");

        builder.HasIndex(a => a.ActionBy)
            .HasDatabaseName("IX_AuditLogs_ActionBy");

        // Índice compuesto para búsquedas comunes por tabla y registro específico
        builder.HasIndex(a => new { a.TableName, a.PrimaryKey })
            .HasDatabaseName("IX_AuditLogs_Table_PrimaryKey");
    }
}
