using System.ComponentModel.DataAnnotations;

namespace SportStore.Web.Entities;

public class Product
{
    public int Id { get; set; }
    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;
    [Required, MaxLength(2000)]
    public string Description { get; set; } = string.Empty;
    [Range(typeof(decimal), "0.01", "100000")]
    public decimal Price { get; set; }
    [Required, MaxLength(250)]
    public string ImagePath { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;
    public int SportId { get; set; }
    public Sport Sport { get; set; } = null!;
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
}
