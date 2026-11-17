using SportStore.Core.Dtos;

namespace SportStore.Core.Services;

public interface IProductCatalogService
{
    Task<HomeDto> GetHomeAsync();
    Task<CatalogDto> GetCatalogAsync(string? search);
    Task<ProductDetailsDto?> GetDetailsAsync(int id);
}
