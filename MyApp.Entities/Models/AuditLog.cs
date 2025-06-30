namespace MyApp.Entities.Models;
/// <summary>
/// Representa un registro de auditoría para rastrear cambios en la base de datos.
/// </summary>
public class AuditLog
{
    /// <summary>
    /// Identificador único del registro de auditoría.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre de la tabla en la que se realizó la acción.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string TableName { get; set; }

    /// <summary>
    /// El tipo de acción realizada (ej. "INSERT", "UPDATE", "DELETE").
    /// </summary>
    [Required]
    [StringLength(20)]
    public string Action { get; set; }

    /// <summary>
    /// La clave primaria del registro afectado, a menudo en formato JSON si es compuesta.
    /// </summary>
    [Required]
    public string PrimaryKey { get; set; }

    /// <summary>
    /// Los valores originales del registro antes del cambio (para UPDATE y DELETE).
    /// Generalmente se almacena como una cadena JSON. Es nulable.
    /// </summary>
    public string? OldValues { get; set; }

    /// <summary>
    /// Los nuevos valores del registro después del cambio (para INSERT y UPDATE).
    /// Generalmente se almacena como una cadena JSON. Es nulable.
    /// </summary>
    public string? NewValues { get; set; }

    /// <summary>
    /// La fecha y hora exactas en que se realizó la acción.
    /// </summary>
    [Required]
    public DateTime ActionDate { get; set; }

    /// <summary>
    /// El identificador del usuario o proceso que realizó la acción.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string ActionBy { get; set; }

    /// <summary>
    /// Descripción adicional o contexto sobre la acción realizada. Es opcional.
    /// </summary>
    [StringLength(200)]
    public string? ActionDescription { get; set; }

    /// <summary>
    /// La dirección IP desde la cual se originó la solicitud. Es opcional.
    /// </summary>
    [StringLength(45)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// El User-Agent del navegador o cliente que realizó la solicitud. Es opcional.
    /// </summary>
    [StringLength(500)]
    public string? UserAgent { get; set; }
}