namespace MyApp.Entities.Interfaces;

/// <summary>
/// Define las propiedades de auditoría para una entidad.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// La fecha y hora en que la entidad fue creada.
    /// </summary>
    DateTime CreatedDate { get; set; }

    /// <summary>
    /// La fecha y hora en que la entidad fue modificada por última vez.
    /// Es nulable, ya que una nueva entidad aún no ha sido modificada.
    /// </summary>
    DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// El identificador (ej. nombre de usuario o ID) de quien creó la entidad.
    /// </summary>
    string CreatedBy { get; set; }

    /// <summary>
    /// El identificador de quien modificó la entidad por última vez.
    /// Es nulable, ya que no tendrá valor al momento de la creación.
    /// </summary>
    string? ModifiedBy { get; set; }
}