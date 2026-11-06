using System.ComponentModel.DataAnnotations;

namespace SportStore.Web.Entities;

public class Sport
{
    public int Id { get; set; }
    [Required, MaxLength(80)]
    public string Name { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
