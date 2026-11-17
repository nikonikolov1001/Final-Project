using Microsoft.EntityFrameworkCore;
using SportStore.Core.Data;

namespace SportStore.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext db)
    {
        await db.Database.MigrateAsync();

        // Re-running initialization preserves the existing catalogue.
        if (await db.Products.AnyAsync()) return;

        db.Products.AddRange(CatalogSeedData.CreateProducts());
        await db.SaveChangesAsync();
    }
}
