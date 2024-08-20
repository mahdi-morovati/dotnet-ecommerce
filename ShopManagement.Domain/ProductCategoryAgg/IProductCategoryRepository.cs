using System.Linq.Expressions;
using _0_framework.Domain;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ShopManagement.Domain.ProductCategoryAgg;

public interface IProductCategoryRepository : IRepository<long, ProductCategory>
{
    public List<ProductCategoryViewModel> GetProductCategories();
    EditProductCategory GetDetails(long id);
    string GetSlugById(long id);
    List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);
}