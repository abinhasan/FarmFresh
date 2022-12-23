using SuperMarket.Domain.DTO;

namespace SuperMarket.Services.Interfaces
{
    public interface IProductService
    {
        Task<int> GetProductCountAsync();
        Task<IReadOnlyList<Product>> GetAllAsync(ProductFilterRequest request);

        Task<Product?> GetAsync(int id);

        Task<Product> CreateAsync(ProductCreateRequest request);
    }
}