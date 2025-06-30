namespace MyApp.DataAccess.Interceptors;

/// <summary>
/// Intercepta las operaciones de guardado de EF Core para gestionar
/// automáticamente los campos de las entidades que implementan <see cref="IAuditableEntity"/>.
/// </summary>
public class AuditInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Se invoca antes de que los cambios se guarden en la base de datos (versión síncrona).
    /// </summary>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// Se invoca antes de que los cambios se guarden en la base de datos (versión asíncrona).
    /// </summary>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Revisa las entidades rastreadas y aplica las reglas de auditoría.
    /// </summary>
    /// <param name="context">El contexto de la base de datos actual.</param>
    private void UpdateAuditFields(DbContext? context)
    {
        if (context == null) return;

        // Obtiene todas las entradas que implementan IAuditableEntity
        var entries = context.ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    // En este punto, los campos CreatedDate y CreatedBy
                    // ya deberían haber sido establecidos en la capa de negocio
                    // o servicio, ya que dependen del usuario y la fecha actual.
                    // El interceptor no los establece para mantener la separación de responsabilidades.
                    break;

                case EntityState.Modified:
                    // Asegura que los campos de creación no se puedan modificar
                    // en una operación de actualización.
                    entry.Property(e => e.CreatedDate).IsModified = false;
                    entry.Property(e => e.CreatedBy).IsModified = false;
                    break;

                    // No se necesita acción para EntityState.Deleted o EntityState.Unchanged
            }
        }
    }
}