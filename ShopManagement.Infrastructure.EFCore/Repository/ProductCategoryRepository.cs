using System.Linq.Expressions;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly ShopContext _context;

    public ProductCategoryRepository(ShopContext context)
    {
        _context = context;
    }

    public void Create(ProductCategory entity)
    {
        _context.ProductCategories.Add(entity);
    }

    public ProductCategory Get(long id)
    {
        return _context.ProductCategories.Find(id);
    }

    public List<ProductCategory> GetAll()
    {
        return _context.ProductCategories.ToList();
    }

    public bool Exists(Expression<Func<ProductCategory, bool>> expression)
    {
        return _context.ProductCategories.Any(expression);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public EditProductCategory GetDetails(long id)
    {
        return _context.ProductCategories.Select(x => new EditProductCategory
        {
            Id = x.Id,
            Description = x.Description,
            Name = x.Name,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Slug = x.Slug
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
    {
        var query = _context.ProductCategories.Select(x => new ProductCategoryViewModel
        {
            Id = x.Id,
            Picture = x.Picture,
            Name = x.Name,
            CreationDate = x.CreationDate.ToString(),
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
        {
            query = query.Where(x => x.Name.Contains(searchModel.Name));
        }

        return query.OrderBy(x => x.Id).ToList();
    } 
}