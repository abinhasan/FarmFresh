using Microsoft.EntityFrameworkCore;
using SuperMarket.Domain.Entities.Contexts;
using SuperMarket.Domain.Interfaces;
using System.Linq.Expressions;

namespace SuperMarket.Infrastructure.Data
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
       where T : class
    {
        private readonly ApplicationDbContext dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
            .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await dbContext
                .Set<T>().CountAsync();
        }

        public async Task<T> FindSingleByAsync(Expression<Func<T, bool>> predicate)
             => await dbContext.Set<T>().FirstOrDefaultAsync(predicate);


        public async Task<IReadOnlyList<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                return await dbContext.Set<T>().Where(predicate).ToListAsync();
            }
            else
            {
                return await dbContext.Set<T>().ToListAsync();
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}