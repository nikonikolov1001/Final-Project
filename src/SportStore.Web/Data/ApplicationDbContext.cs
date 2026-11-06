using Microsoft.EntityFrameworkCore;
using SportStore.Web.Entities;

namespace SportStore.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Sport> Sports => Set<Sport>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<Brand>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<Sport>().HasIndex(x => x.Name).IsUnique();

        builder.Entity<Product>(product =>
        {
            product.Property(x => x.Price).HasPrecision(18, 2);
            product.HasOne(x => x.Category).WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
            product.HasOne(x => x.Brand).WithMany(x => x.Products)
                .HasForeignKey(x => x.BrandId).OnDelete(DeleteBehavior.Restrict);
            product.HasOne(x => x.Sport).WithMany(x => x.Products)
                .HasForeignKey(x => x.SportId).OnDelete(DeleteBehavior.Restrict);
            product.ToTable(table => table.HasCheckConstraint("CK_Products_Price", "[Price] > 0"));
        });
        builder.Entity<ProductVariant>(variant =>
        {
            variant.HasIndex(x => new { x.ProductId, x.Size, x.Color }).IsUnique();
            variant.ToTable(table => table.HasCheckConstraint("CK_ProductVariants_Stock", "[Stock] >= 0"));
        });
    }
}
