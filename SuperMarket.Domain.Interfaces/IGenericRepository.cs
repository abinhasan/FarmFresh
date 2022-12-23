using System.Linq.Expressions;

namespace SuperMarket.Domain.Interfaces
{
    public interface IGenericRepository<T>
        where T : class
    {
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);

        Task<int> GetCountAsync();

        Task<T> FindSingleByAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> FindByAsync(Expression<Func<T, bool>> predicate = null);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}