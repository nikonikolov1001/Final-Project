using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SportStore.Core.Services;
using SportStore.Web.Models;

namespace SportStore.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductCatalogService catalog;
    public HomeController(IProductCatalogService catalog) { this.catalog = catalog; }

    public async Task<IActionResult> Index() => View(await catalog.GetHomeAsync());

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        Response.StatusCode = 500;
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [ActionName("StatusCode")]
    public IActionResult ShowStatusCode(int code)
    {
        Response.StatusCode = code;
        return code == 404 ? View("NotFound") : View("Error", new ErrorViewModel());
    }
}
