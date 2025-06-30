using CustomPagedResult = MyApp.Business.Dtos.Common.PagedResult<MyApp.Business.Dtos.Item.ItemDto>;

namespace MyApp.Presentation.Controllers;

/// <summary>
/// Gestiona las operaciones CRUD y las consultas para los artículos (Items).
/// Actualizado para cumplir RF2.4: Listar y paginar artículos con ordenamiento.
/// </summary>
[Authorize]
public class ItemsController : Controller
{
    private readonly IItemService _itemService;
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(IItemService itemService, ILogger<ItemsController> logger)
    {
        _itemService = itemService;
        _logger = logger;
    }

    /// <summary>
    /// RF2.4: Muestra la lista paginada de artículos con filtros y ordenamiento
    /// </summary>
    public async Task<IActionResult> Index(
        string? search,
        string? category,
        ItemStatus? status,
        string? sortBy,
        string? sortOrder,
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            // Validar parámetros básicos
            if (page < 1) page = 1;
            if (pageSize < 5 || pageSize > 100) pageSize = 10;
            if (string.IsNullOrEmpty(sortBy)) sortBy = "Name";
            if (string.IsNullOrEmpty(sortOrder)) sortOrder = "asc";

            var parameters = new ItemFilterParameters
            {
                Page = page,
                PageSize = pageSize,
                Search = search,
                Category = category,
                Status = status,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            var pagedItems = await _itemService.GetItemsPagedAsync(parameters);
            var categories = await _itemService.GetCategoriesAsync();

            var model = new ItemsIndexViewModel
            {
                PagedItems = pagedItems,
                SearchTerm = search,
                SelectedCategory = category,
                SelectedStatus = status,
                Categories = categories.ToList(),
                SortBy = sortBy,
                SortOrder = sortOrder,
                PageSize = pageSize
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando artículos paginados");
            TempData["ErrorMessage"] = "Error cargando artículos. Por favor, inténtalo de nuevo.";

            // Retornar modelo vacío en caso de error
            var emptyModel = new ItemsIndexViewModel
            {
                Categories = new List<string>()
            };
            return View(emptyModel);
        }
    }

    /// <summary>
    /// API endpoint para cambiar el tamaño de página dinámicamente
    /// </summary>
    [HttpPost]
    public IActionResult ChangePageSize(int pageSize, string? search, string? category, ItemStatus? status, string? sortBy, string? sortOrder)
    {
        return RedirectToAction(nameof(Index), new
        {
            search,
            category,
            status,
            sortBy,
            sortOrder,
            page = 1,
            pageSize
        });
    }

    // ... resto de métodos CRUD permanecen iguales ...

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var item = await _itemService.GetItemByIdAsync(id);
            return View(item);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando detalles del artículo para ID {ItemId}", id);
            return View("Error");
        }
    }

    [Authorize(Roles = "Administrator")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateItemViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var createDto = new CreateItemDto
            {
                Code = model.Code,
                Name = model.Name,
                Category = model.Category,
                Status = model.Status,
                Location = model.Location
            };

            await _itemService.CreateItemAsync(createDto);
            _logger.LogInformation("Artículo creado: {ItemCode}", model.Code);
            TempData["SuccessMessage"] = "¡Artículo creado exitosamente!";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando artículo");
            ModelState.AddModelError(string.Empty, "Ocurrió un error al crear el artículo.");
            return View(model);
        }
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var item = await _itemService.GetItemByIdAsync(id);
            var model = new EditItemViewModel
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Category = item.Category,
                Status = item.Status,
                Location = item.Location
            };
            return View(model);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando artículo para editar: {ItemId}", id);
            return View("Error");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditItemViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var updateDto = new UpdateItemDto
            {
                Code = model.Code,
                Name = model.Name,
                Category = model.Category,
                Status = model.Status,
                Location = model.Location
            };

            await _itemService.UpdateItemAsync(id, updateDto);
            _logger.LogInformation("Artículo actualizado: {ItemId}", id);
            TempData["SuccessMessage"] = "¡Artículo actualizado exitosamente!";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando artículo: {ItemId}", id);
            ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar el artículo.");
            return View(model);
        }
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var item = await _itemService.GetItemByIdAsync(id);
            return View(item);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando artículo para eliminar: {ItemId}", id);
            return View("Error");
        }
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var success = await _itemService.DeleteItemAsync(id);
            if (success)
            {
                _logger.LogInformation("Artículo eliminado: {ItemId}", id);
                TempData["SuccessMessage"] = "¡Artículo eliminado exitosamente!";
            }
            else
            {
                TempData["ErrorMessage"] = "Artículo no encontrado.";
            }
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Delete), new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando artículo: {ItemId}", id);
            TempData["ErrorMessage"] = "Ocurrió un error al eliminar el artículo.";
            return RedirectToAction(nameof(Delete), new { id });
        }
    }

    [HttpGet]
    public async Task<IActionResult> SearchApi(string term)
    {
        try
        {
            if (string.IsNullOrEmpty(term) || term.Length < 2)
                return Json(new List<object>());

            var items = await _itemService.SearchItemsAsync(term);
            var results = items.Take(10).Select(i => new
            {
                id = i.Id,
                code = i.Code,
                name = i.Name,
                category = i.Category,
                status = i.Status.ToString()
            });

            return Json(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en API de búsqueda");
            return Json(new List<object>());
        }
    }
}