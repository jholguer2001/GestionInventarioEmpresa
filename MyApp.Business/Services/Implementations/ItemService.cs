using MyApp.Business.Dtos.Common;

namespace MyApp.Business.Services.Implementations;

/// <summary>
/// Implementación del servicio de negocio para la gestión de artículos (Items).
/// Actualizado con paginación y ordenamiento para cumplir RF2.4.
/// </summary>
public class ItemService : IItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditService _auditService;

    public ItemService(IUnitOfWork unitOfWork, IAuditService auditService)
    {
        _unitOfWork = unitOfWork;
        _auditService = auditService;
    }

    /// <summary>
    /// RF2.4: Implementación de paginación con filtros y ordenamiento
    /// </summary>
    public async Task<PagedResult<ItemDto>> GetItemsPagedAsync(ItemFilterParameters parameters)
    {
        var allItems = await _unitOfWork.Items.GetAllAsync();
        var query = allItems.AsQueryable();

        // Aplicar filtros
        if (!string.IsNullOrEmpty(parameters.Search))
        {
            query = query.Where(i =>
                i.Name.Contains(parameters.Search, StringComparison.OrdinalIgnoreCase) ||
                i.Code.Contains(parameters.Search, StringComparison.OrdinalIgnoreCase) ||
                i.Category.Contains(parameters.Search, StringComparison.OrdinalIgnoreCase) ||
                (i.Location != null && i.Location.Contains(parameters.Search, StringComparison.OrdinalIgnoreCase)));
        }

        if (!string.IsNullOrEmpty(parameters.Category))
        {
            query = query.Where(i => i.Category.Equals(parameters.Category, StringComparison.OrdinalIgnoreCase));
        }

        if (parameters.Status.HasValue)
        {
            query = query.Where(i => i.Status == parameters.Status.Value);
        }

        // Aplicar ordenamiento
        query = parameters.SortBy.ToLower() switch
        {
            "code" => parameters.SortOrder == "desc"
                ? query.OrderByDescending(i => i.Code)
                : query.OrderBy(i => i.Code),
            "category" => parameters.SortOrder == "desc"
                ? query.OrderByDescending(i => i.Category)
                : query.OrderBy(i => i.Category),
            "status" => parameters.SortOrder == "desc"
                ? query.OrderByDescending(i => i.Status)
                : query.OrderBy(i => i.Status),
            "createddate" => parameters.SortOrder == "desc"
                ? query.OrderByDescending(i => i.CreatedDate)
                : query.OrderBy(i => i.CreatedDate),
            _ => parameters.SortOrder == "desc" // Por defecto ordenar por Name
                ? query.OrderByDescending(i => i.Name)
                : query.OrderBy(i => i.Name)
        };

        // Contar total de elementos
        var totalItems = query.Count();

        // Aplicar paginación
        var items = query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(MapToDto)
            .ToList();

        return new PagedResult<ItemDto>
        {
            Items = items,
            CurrentPage = parameters.Page,
            PageSize = parameters.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / parameters.PageSize)
        };
    }

    /// <summary>
    /// Obtiene todas las categorías disponibles
    /// </summary>
    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        var items = await _unitOfWork.Items.GetAllAsync();
        return items.Select(i => i.Category).Distinct().OrderBy(c => c);
    }

    /// <summary>
    /// Mapea una entidad Item a su correspondiente ItemDto.
    /// </summary>
    private ItemDto MapToDto(Item item)
    {
        return new ItemDto
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Category = item.Category,
            Status = item.Status,
            Location = item.Location,
            CreatedDate = item.CreatedDate
        };
    }

    // ... resto de métodos existentes permanecen igual ...

    public async Task<IEnumerable<ItemDto>> GetAllItemsAsync()
    {
        var items = await _unitOfWork.Items.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<ItemDto> GetItemByIdAsync(int id)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(id);
        if (item == null)
            throw new KeyNotFoundException($"Item with ID {id} not found");

        return MapToDto(item);
    }

    public async Task<ItemDto> CreateItemAsync(CreateItemDto createItemDto)
    {
        if (await _unitOfWork.Items.CodeExistsAsync(createItemDto.Code))
        {
            throw new InvalidOperationException($"Item code '{createItemDto.Code}' already exists");
        }

        var item = new Item
        {
            Code = createItemDto.Code,
            Name = createItemDto.Name,
            Category = createItemDto.Category,
            Status = createItemDto.Status,
            Location = createItemDto.Location
        };

        await _unitOfWork.Items.AddAsync(item);
        await _unitOfWork.SaveChangesAsync();

        await _auditService.LogActionAsync(
            "Items", "CREATE", item.Id.ToString(),
            null, new { item.Code, item.Name, item.Category, item.Status, item.Location },
            "Item created");

        return MapToDto(item);
    }

    public async Task<ItemDto> UpdateItemAsync(int id, UpdateItemDto updateItemDto)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(id);
        if (item == null)
            throw new KeyNotFoundException($"Item with ID {id} not found");

        var oldValues = new { item.Code, item.Name, item.Category, item.Status, item.Location };

        if (await _unitOfWork.Items.CodeExistsForOtherItemAsync(updateItemDto.Code, id))
        {
            throw new InvalidOperationException($"Item code '{updateItemDto.Code}' already exists");
        }

        item.Code = updateItemDto.Code;
        item.Name = updateItemDto.Name;
        item.Category = updateItemDto.Category;
        item.Status = updateItemDto.Status;
        item.Location = updateItemDto.Location;

        await _unitOfWork.Items.UpdateAsync(item);
        await _unitOfWork.SaveChangesAsync();

        var newValues = new { item.Code, item.Name, item.Category, item.Status, item.Location };
        await _auditService.LogActionAsync(
            "Items", "UPDATE", item.Id.ToString(),
            oldValues, newValues, "Item updated");

        return MapToDto(item);
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(id);
        if (item == null)
            return false;

        var activeLoans = await _unitOfWork.Loans.GetActiveLoansByItemAsync(id);
        if (activeLoans.Any())
        {
            throw new InvalidOperationException("Cannot delete item with active loans");
        }

        var oldValues = new { item.Code, item.Name, item.Category, item.Status };

        await _unitOfWork.Items.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        await _auditService.LogActionAsync(
            "Items", "DELETE", id.ToString(),
            oldValues, null, "Item deleted");

        return true;
    }

    public async Task<IEnumerable<ItemDto>> SearchItemsAsync(string searchTerm)
    {
        var items = await _unitOfWork.Items.SearchAsync(searchTerm);
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<ItemDto>> GetItemsByCategoryAsync(string category)
    {
        var items = await _unitOfWork.Items.GetByCategoryAsync(category);
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<ItemDto>> GetItemsByStatusAsync(ItemStatus status)
    {
        var items = await _unitOfWork.Items.GetByStatusAsync(status);
        return items.Select(MapToDto);
    }

    public async Task<bool> IsItemAvailableForLoanAsync(int itemId)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(itemId);
        if (item == null || item.Status != ItemStatus.Available)
            return false;

        var activeLoans = await _unitOfWork.Loans.GetActiveLoansByItemAsync(itemId);
        return !activeLoans.Any();
    }

    public async Task<IEnumerable<ItemDto>> GetFilteredItemsAsync(string? searchTerm, string? category, ItemStatus? status)
    {
        var items = await _unitOfWork.Items.GetFilteredAsync(searchTerm, category, status);
        return items.Select(MapToDto);
    }
}