using _0_framework.Domain;
using ShopManagement.Application.Contracts.ProductPicture;

namespace ShopManagement.Domain.ProductPictureAgg;

public interface IProductPictureRepository : IRepository<long, ProductPicture>
{
    EditProductPicture GetDetails(long id);
    ProductPicture GetWithProductAndCategory(long id);
    List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel);
}