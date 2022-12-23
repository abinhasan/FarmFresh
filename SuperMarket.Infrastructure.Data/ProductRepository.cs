using SuperMarket.Domain.Entities.Contexts;
using SuperMarket.Domain.Entities.Entities;
using SuperMarket.Domain.Interfaces;

namespace SuperMarket.Infrastructure.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
