using SuperMarket.Domain.DTO;

namespace SuperMarket.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetAllAsync();

        Task<Category?> GetAsync(int id);

        Task<Category> CreateAsync(CategoryCreateRequest request);
    }
}
