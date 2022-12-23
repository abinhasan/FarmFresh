using SuperMarket.Domain.DTO;
using SuperMarket.Domain.Interfaces;
using SuperMarket.Services.Interfaces;

namespace SuperMarket.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync()
        {
            var entity = await categoryRepository.FindByAsync();
            return entity.Select(Map).ToList();
        }

        public async Task<Category?> GetAsync(int id)
        {
            var entity = await categoryRepository.FindSingleByAsync(d => d.Id == id);

            return entity != null
                ? Map(entity)
                : null;
        }

        public async Task<Category> CreateAsync(CategoryCreateRequest request)
        {
            var entity = new Domain.Entities.Entities.Category
            {
                Name = request.Name,
                IsActive = request.IsActive
            };

            entity = await categoryRepository.AddAsync(entity);

            return Map(entity);
        }

        private Category Map(Domain.Entities.Entities.Category entity)
        {
            return new Category
            {
                Name = entity.Name,
                IsActive = entity.IsActive
            };
        }
    }
}
