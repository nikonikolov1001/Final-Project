using Microsoft.EntityFrameworkCore;
using SportStore.Web.Data;
using SportStore.Web.Entities;
using SportStore.Web.ViewModels;

namespace SportStore.Web.Services;

public class ProductCatalogService
{
    private readonly ApplicationDbContext db;

    public ProductCatalogService(ApplicationDbContext db)
    {
        this.db = db;
    }

    public async Task<HomeViewModel> GetHomeAsync()
    {
        return new HomeViewModel
        {
            FeaturedProducts = await Cards(db.Products.AsNoTracking())
                .OrderBy(x => x.Id).Take(3).ToListAsync(),
            ProductCount = await db.Products.CountAsync()
        };
    }

    public async Task<CatalogViewModel> GetCatalogAsync(string? search)
    {
        IQueryable<Product> products = db.Products.AsNoTracking();
        search = search?.Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search[..Math.Min(search.Length, 100)];
            string term = search.ToLower();
            products = products.Where(x => x.Name.ToLower().Contains(term));
        }

        return new CatalogViewModel
        {
            Search = search,
            Products = await Cards(products).OrderBy(x => x.Name).ToListAsync()
        };
    }

    public async Task<ProductDetailsViewModel?> GetDetailsAsync(int id)
    {
        return await db.Products.AsNoTracking().Where(x => x.Id == id)
            .Select(x => new ProductDetailsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Brand = x.Brand.Name,
                Category = x.Category.Name,
                Sport = x.Sport.Name,
                Price = x.Price,
                ImagePath = x.ImagePath,
                Variants = x.Variants.OrderBy(v => v.Size).ThenBy(v => v.Color)
                    .Select(v => new VariantViewModel
                    {
                        Id = v.Id, Size = v.Size, Color = v.Color, Stock = v.Stock
                    }).ToList()
            }).SingleOrDefaultAsync();
    }

    private static IQueryable<ProductCardViewModel> Cards(IQueryable<Product> products)
    {
        return products.Select(x => new ProductCardViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Brand = x.Brand.Name,
            Category = x.Category.Name,
            Sport = x.Sport.Name,
            Price = x.Price,
            ImagePath = x.ImagePath,
            TotalStock = x.Variants.Sum(v => (int?)v.Stock) ?? 0
        });
    }
}
