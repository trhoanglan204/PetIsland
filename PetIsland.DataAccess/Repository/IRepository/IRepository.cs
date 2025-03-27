using System.Linq.Expressions;
namespace PetIsland.DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    // T - Category    
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}
