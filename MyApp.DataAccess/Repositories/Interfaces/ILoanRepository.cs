namespace MyApp.DataAccess.Repositories.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de préstamos (Loans),
/// extendiendo las operaciones CRUD básicas con consultas específicas.
/// </summary>
public interface ILoanRepository : IRepository<Loan>
{
    /// <summary>
    /// Obtiene todos los préstamos asociados a un usuario específico.
    /// </summary>
    /// <param name="userId">El ID del usuario.</param>
    /// <returns>Una colección de los préstamos del usuario.</returns>
    Task<IEnumerable<Loan>> GetByUserAsync(int userId);

    /// <summary>
    /// Obtiene todos los préstamos (históricos y actuales) de un artículo específico.
    /// </summary>
    /// <param name="itemId">El ID del artículo.</param>
    /// <returns>Una colección de préstamos asociados al artículo.</returns>
    Task<IEnumerable<Loan>> GetByItemAsync(int itemId);

    /// <summary>
    /// Obtiene todos los préstamos que se encuentran en un estado específico.
    /// </summary>
    /// <param name="status">El estado del préstamo a filtrar.</param>
    /// <returns>Una colección de préstamos con el estado especificado.</returns>
    Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status);

    /// <summary>
    /// Obtiene los préstamos activos para un artículo específico.
    /// </summary>
    /// <param name="itemId">El ID del artículo.</param>
    /// <returns>Una colección de préstamos activos (normalmente será uno o ninguno).</returns>
    Task<IEnumerable<Loan>> GetActiveLoansByItemAsync(int itemId);

    /// <summary>
    /// Obtiene el préstamo activo actual de un artículo.
    /// </summary>
    /// <param name="itemId">El ID del artículo.</param>
    /// <returns>El préstamo activo, o nulo si el artículo no está prestado.</returns>
    Task<Loan?> GetCurrentLoanByItemAsync(int itemId);

    /// <summary>
    /// Obtiene todos los préstamos que están pendientes de aprobación o entrega.
    /// </summary>
    /// <returns>Una colección de préstamos pendientes.</returns>
    Task<IEnumerable<Loan>> GetPendingLoansAsync();

    /// <summary>
    /// Obtiene todos los préstamos activos que ya han pasado su fecha de devolución esperada.
    /// </summary>
    /// <returns>Una colección de préstamos vencidos.</returns>
    Task<IEnumerable<Loan>> GetOverdueLoansAsync();

    /// <summary>
    /// Obtiene todos los préstamos realizados dentro de un rango de fechas.
    /// </summary>
    /// <param name="fromDate">La fecha de inicio del rango.</param>
    /// <param name="toDate">La fecha de fin del rango.</param>
    /// <returns>Una colección de préstamos.</returns>
    Task<IEnumerable<Loan>> GetLoansByDateRangeAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Verifica si un artículo tiene actualmente un préstamo activo.
    /// </summary>
    /// <param name="itemId">El ID del artículo a verificar.</param>
    /// <returns>True si el artículo está actualmente prestado; de lo contrario, false.</returns>
    Task<bool> HasActiveLoanAsync(int itemId);

    /// <summary>
    /// Cuenta el número de préstamos activos que tiene un usuario.
    /// </summary>
    /// <param name="userId">El ID del usuario.</param>
    /// <returns>El número de préstamos activos del usuario.</returns>
    Task<int> GetActiveLoanCountByUserAsync(int userId);
}