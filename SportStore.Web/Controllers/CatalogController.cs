using Microsoft.AspNetCore.Mvc;
using SportStore.Core.Services;

namespace SportStore.Web.Controllers;

public class CatalogController : Controller
{
    private readonly IProductCatalogService catalog;
    public CatalogController(IProductCatalogService catalog) { this.catalog = catalog; }

    public async Task<IActionResult> Index(string? search)
        => View(await catalog.GetCatalogAsync(search));

    public async Task<IActionResult> Details(int id)
    {
        var product = await catalog.GetDetailsAsync(id);
        return product == null ? NotFound() : View(product);
    }
}
