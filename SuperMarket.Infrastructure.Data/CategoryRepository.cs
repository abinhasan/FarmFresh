using SuperMarket.Domain.Entities.Contexts;
using SuperMarket.Domain.Entities.Entities;
using SuperMarket.Domain.Interfaces;

namespace SuperMarket.Infrastructure.Data
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
