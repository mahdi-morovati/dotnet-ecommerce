using System.Linq.Expressions;
using _0_framework.Domain;
using Microsoft.EntityFrameworkCore;

namespace _0_framework.Infrastructure;

public class RepositoryBase<Tkey, T>: IRepository<Tkey, T> where T : class
{
    private readonly DbContext _context;

    public RepositoryBase(DbContext context)
    {
        _context = context;
    }

    public T Get(Tkey id)
    {
        return _context.Find<T>(id);
    }

    public List<T> Get()
    {
        return _context.Set<T>().ToList();
    }

    public void Create(T entity)
    {
        _context.Add(entity); // equals to _context.Add<T>(entity);
    }

    public void SaveChanges()
    {
        throw new NotImplementedException();
    }

    public bool Exists(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Any(expression);
    }
}