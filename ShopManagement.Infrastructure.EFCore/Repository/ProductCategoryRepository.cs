using System.Linq.Expressions;
using _0_framework.Application;
using _0_framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository;

public class ProductCategoryRepository : RepositoryBase<long, ProductCategory>, IProductCategoryRepository
{
    private readonly ShopContext _context;

    public ProductCategoryRepository(ShopContext context) : base(context)
    {
        _context = context;
    }

    public List<ProductCategoryViewModel> GetProductCategories()
    {
        return _context.ProductCategories.Select(x => new ProductCategoryViewModel
        {
            Id = x.Id,
            Name = x.Name,
        }).ToList();
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
            // Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Slug = x.Slug
        }).FirstOrDefault(x => x.Id == id);
    }

    public string GetSlugById(long id)
    {
        return _context.ProductCategories.Select(x => new { x.Id, x.Slug }).FirstOrDefault(x => x.Id == id).Slug;
    }

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
    {
        var query = _context.ProductCategories.Select(x => new ProductCategoryViewModel
        {
            Id = x.Id,
            Picture = x.Picture,
            Name = x.Name,
            CreationDate = x.CreationDate.ToFarsi(),
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
        {
            query = query.Where(x => x.Name.Contains(searchModel.Name));
        }

        return query.OrderBy(x => x.Id).ToList();
    }
}