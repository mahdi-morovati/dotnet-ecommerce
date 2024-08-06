using System.Linq.Expressions;

namespace _0_framework.Domain;

/**
 * Tkey is type of The Id
 * T is type of the entity
 */
public interface IRepository<TKey, T> where T : class
{
    T Get(TKey id);
    List<T> Get();
    void Create(T entity);
    void SaveChanges();
    bool Exists(Expression<Func<T, bool>> expression);
}