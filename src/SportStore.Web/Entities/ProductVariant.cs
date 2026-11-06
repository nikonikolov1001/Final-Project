using System.ComponentModel.DataAnnotations;

namespace SportStore.Web.Entities;

public class ProductVariant
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    [Required, MaxLength(30)]
    public string Size { get; set; } = string.Empty;
    [Required, MaxLength(50)]
    public string Color { get; set; } = string.Empty;
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
