namespace SportStore.Core.Dtos;

public class CatalogDto
{
    public IReadOnlyList<ProductCardDto> Products { get; set; } = [];
    public string? Search { get; set; }
}
