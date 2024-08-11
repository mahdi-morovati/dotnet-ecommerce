using _01_LampshadeQuery.Contracts.ProductCategory;
using ShopManagement.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class ProductCategoryQuery : IProductCategoryQuery
{
    private readonly ShopContext _context;

    public ProductCategoryQuery(ShopContext context)
    {
        _context = context;
    }

    public List<ProductCategoryQueryModel> GetProductCategories()
    {
        return _context.ProductCategories.Select(x => new ProductCategoryQueryModel
        {
            Id = x.Id,
            Name = x.Name,
            Picture = x.Picture,
            PictureTitle = x.PictureTitle,
            PictureAlt = x.PictureAlt,
            Slug = x.Slug,
        }).ToList();
    }
}