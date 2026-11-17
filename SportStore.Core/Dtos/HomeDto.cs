namespace SportStore.Core.Dtos;

public class HomeDto
{
    public IReadOnlyList<ProductCardDto> FeaturedProducts { get; set; } = [];
    public int ProductCount { get; set; }
}
