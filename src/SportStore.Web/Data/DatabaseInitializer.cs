using Microsoft.EntityFrameworkCore;
using SportStore.Web.Entities;

namespace SportStore.Web.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        // Keep existing catalogue data when initialization is run again.
        if (await db.Products.AnyAsync()) return;

        var shoes = new Category { Name = "Обувки" };
        var clothes = new Category { Name = "Облекло" };
        var equipment = new Category { Name = "Екипировка" };
        var aerofit = new Brand { Name = "AeroFit" };
        var peak = new Brand { Name = "Peak" };
        var core = new Brand { Name = "CoreSport" };
        var running = new Sport { Name = "Бягане" };
        var football = new Sport { Name = "Футбол" };
        var fitness = new Sport { Name = "Фитнес" };

        db.Products.AddRange(
            CreateProduct("Обувки Aero Run", "Леки обувки за ежедневно бягане с дишаща горна част.",
                79.90m, "shoes", shoes, aerofit, running, ("42", "Син", 8), ("43", "Син", 5)),
            CreateProduct("Тениска Active Dry", "Спортна тениска с бързосъхнеща материя за тренировки.",
                24.90m, "shirt", clothes, aerofit, fitness, ("M", "Оранжев", 12), ("L", "Оранжев", 9)),
            CreateProduct("Футболна топка Match Pro", "Устойчива топка за тренировки и свободна игра.",
                32.00m, "ball", equipment, peak, football, ("5", "Бял", 15), ("4", "Бял", 7)),
            CreateProduct("Постелка Balance", "Неплъзгаща се постелка за упражнения у дома.",
                29.90m, "mat", equipment, core, fitness, ("6 мм", "Зелен", 0), ("6 мм", "Син", 0)),
            CreateProduct("Раница Daily Sport", "Спортна раница с отделение за екип и принадлежности.",
                35.00m, "backpack", equipment, core, fitness, ("20 л", "Черен", 6), ("20 л", "Оранжев", 4)),
            CreateProduct("Дъмбел Core 5", "Дъмбел с удобно покритие за домашни силови тренировки.",
                19.90m, "dumbbell", equipment, peak, fitness, ("5 кг", "Черен", 14), ("3 кг", "Черен", 11))
        );
        await db.SaveChangesAsync();
    }

    private static Product CreateProduct(string name, string description, decimal price, string image,
        Category category, Brand brand, Sport sport,
        params (string Size, string Color, int Stock)[] variants)
    {
        return new Product
        {
            Name = name,
            Description = description,
            Price = price,
            ImagePath = $"/images/{image}.svg",
            Category = category,
            Brand = brand,
            Sport = sport,
            Variants = variants.Select(x => new ProductVariant
            {
                Size = x.Size,
                Color = x.Color,
                Stock = x.Stock
            }).ToList()
        };
    }
}
