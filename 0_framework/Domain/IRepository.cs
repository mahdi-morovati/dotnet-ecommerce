using System.Linq.Expressions;

namespace _0_framework.Domain;

/// <summary>
/// this class is used to create repository
/// </summary>
/// <typeparam name="TKey">The type of the id of the entity</typeparam>
/// <typeparam name="T">Class of The entity</typeparam>
public interface IRepository<TKey, T> where T : class
{
    T Get(TKey id);
    List<T> Get();
    void Create(T entity);
    void SaveChanges();
    bool Exists(Expression<Func<T, bool>> expression);
}