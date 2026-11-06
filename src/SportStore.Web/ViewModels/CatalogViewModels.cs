namespace SportStore.Web.ViewModels;

public class ProductCardViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Sport { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public int TotalStock { get; set; }
}

public class CatalogViewModel
{
    public IReadOnlyList<ProductCardViewModel> Products { get; set; } = [];
    public string? Search { get; set; }
}

public class HomeViewModel
{
    public IReadOnlyList<ProductCardViewModel> FeaturedProducts { get; set; } = [];
    public int ProductCount { get; set; }
}

public class ProductDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Sport { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public IReadOnlyList<VariantViewModel> Variants { get; set; } = [];
}

public class VariantViewModel
{
    public int Id { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Stock { get; set; }
}

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
