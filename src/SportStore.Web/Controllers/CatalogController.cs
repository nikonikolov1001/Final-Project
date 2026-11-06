using Microsoft.AspNetCore.Mvc;
using SportStore.Web.Services;

namespace SportStore.Web.Controllers;

public class CatalogController : Controller
{
    private readonly ProductCatalogService catalog;
    public CatalogController(ProductCatalogService catalog) { this.catalog = catalog; }

    public async Task<IActionResult> Index(string? search)
        => View(await catalog.GetCatalogAsync(search));

    public async Task<IActionResult> Details(int id)
    {
        var product = await catalog.GetDetailsAsync(id);
        return product == null ? NotFound() : View(product);
    }
}
