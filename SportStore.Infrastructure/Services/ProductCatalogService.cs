using Microsoft.EntityFrameworkCore;
using SportStore.Infrastructure.Data;
using SportStore.Core.Models;
using SportStore.Core.Dtos;
using SportStore.Core.Services;

namespace SportStore.Infrastructure.Services;

public class ProductCatalogService : IProductCatalogService
{
    private readonly ApplicationDbContext db;

    public ProductCatalogService(ApplicationDbContext db)
    {
        this.db = db;
    }

    public async Task<HomeDto> GetHomeAsync()
    {
        return new HomeDto
        {
            FeaturedProducts = await Cards(db.Products.AsNoTracking())
                .OrderBy(x => x.Id).Take(3).ToListAsync(),
            ProductCount = await db.Products.CountAsync()
        };
    }

    public async Task<CatalogDto> GetCatalogAsync(string? search)
    {
        IQueryable<Product> products = db.Products.AsNoTracking();
        search = search?.Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search[..Math.Min(search.Length, 100)];
            string term = search.ToLower();
            products = products.Where(x => x.Name.ToLower().Contains(term));
        }

        return new CatalogDto
        {
            Search = search,
            Products = await Cards(products).OrderBy(x => x.Name).ToListAsync()
        };
    }

    public async Task<ProductDetailsDto?> GetDetailsAsync(int id)
    {
        return await db.Products.AsNoTracking().Where(x => x.Id == id)
            .Select(x => new ProductDetailsDto
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
                    .Select(v => new ProductVariantDto
                    {
                        Id = v.Id, Size = v.Size, Color = v.Color, Stock = v.Stock
                    }).ToList()
            }).SingleOrDefaultAsync();
    }

    private static IQueryable<ProductCardDto> Cards(IQueryable<Product> products)
    {
        return products.Select(x => new ProductCardDto
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
