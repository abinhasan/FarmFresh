using SuperMarket.Domain.DTO;
using SuperMarket.Domain.Interfaces;
using SuperMarket.Services.Interfaces;

namespace SuperMarket.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<int> GetProductCountAsync()
        {
            return await productRepository.GetCountAsync();
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync(ProductFilterRequest request)
        {
            var entity = await productRepository.GetPagedReponseAsync(request.PageNumber, request.PageSize);

            return entity.Select(Map).ToList();
        }

        public async Task<Product?> GetAsync(int id)
        {
            var entity = await productRepository.FindSingleByAsync(d => d.Id == id);

            return entity != null
                ? Map(entity)
                : null;
        }

        public async Task<Product> CreateAsync(ProductCreateRequest request)
        {
            var entity = new Domain.Entities.Entities.Product
            {
                Title = request.Title,
                Code = request.Code,
                Sku = request.Sku,
                Price = request.Price,
                Description = request.Description,
                IsActive = request.IsActive,
                CategoryId = request.CategoryId 
            };

            entity = await productRepository.AddAsync(entity);

            return Map(entity);  
        }

        private Product Map(Domain.Entities.Entities.Product entity)
        {
            return new Product
            {
                Title = entity.Title,
                Code = entity.Code,
                Sku = entity.Sku,
                Price = entity.Price,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
        }
    }
}