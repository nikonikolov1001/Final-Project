using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SportStore.Core.Data;
using SportStore.Infrastructure.Data;
using SportStore.Infrastructure.Services;
using SportStore.Web.Controllers;

namespace SportStore.Tests;

[TestFixture]
public class CatalogTests
{
    private ApplicationDbContext database = null!;
    private ProductCatalogService catalog = null!;

    [SetUp]
    public async Task SetUp()
    {
        // A separate database keeps changes in one test out of the other tests.
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        database = new ApplicationDbContext(options);
        database.Products.AddRange(CatalogSeedData.CreateProducts());
        await database.SaveChangesAsync();

        catalog = new ProductCatalogService(database);
    }

    [TearDown]
    public async Task TearDown()
    {
        await database.DisposeAsync();
    }

    [Test]
    public async Task CatalogSearchIgnoresCaseAndOuterWhitespace()
    {
        var result = await catalog.GetCatalogAsync("  AERO  ");

        Assert.That(result.Products.Select(product => product.Name),
            Is.EqualTo(new[] { "Обувки Aero Run" }));
        Assert.That(result.Search, Is.EqualTo("AERO"));
    }

    [Test]
    public async Task DetailsKeepOutOfStockVariantsAndReturnNullForUnknownProduct()
    {
        var mat = await database.Products.SingleAsync(
            product => product.ImagePath == "/images/mat.svg");

        var details = await catalog.GetDetailsAsync(mat.Id);
        var missingProduct = await catalog.GetDetailsAsync(int.MaxValue);

        Assert.That(details, Is.Not.Null);
        Assert.That(details!.Variants, Has.Count.EqualTo(2));
        Assert.That(details.Variants.All(variant => variant.Stock == 0), Is.True);
        Assert.That(details.Variants.Select(variant => variant.Color),
            Is.EquivalentTo(new[] { "Зелен", "Син" }));
        Assert.That(missingProduct, Is.Null);
    }

    [Test]
    public async Task HomeShowsThreeFeaturedProductsAndCountsTheWholeCatalog()
    {
        var home = await catalog.GetHomeAsync();

        Assert.That(home.ProductCount, Is.EqualTo(6));
        Assert.That(home.FeaturedProducts, Has.Count.EqualTo(3));
        Assert.That(home.FeaturedProducts.Select(product => product.Id), Is.Unique);
    }

    [Test]
    public async Task DetailsActionReturnsNotFoundForUnknownProduct()
    {
        var controller = new CatalogController(catalog);

        var result = await controller.Details(int.MaxValue);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }
}
