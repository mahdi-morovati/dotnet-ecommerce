using _0_framework.Domain;
using ShopManagement.Application.Contracts.Product;

namespace ShopManagement.Domain.ProductAgg;

public interface IProductRepository : IRepository<long, Product>
{
    Product GetWithCategory(long id);
    EditProduct GetDetails(long id);
    List<ProductViewModel> Search(ProductSearchModel searchModel);
    List<ProductViewModel> GetProducts();
}