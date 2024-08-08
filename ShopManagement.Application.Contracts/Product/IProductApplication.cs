using _0_framework.Application;

namespace ShopManagement.Application.Contracts.Product;

public interface IProductApplication
{
    OperationResult Create(CreateProduct command);
    OperationResult Edit(EditProduct command);
    EditProduct GetDetails(long id);
    List<ProductViewModel> Search(ProductSearchModel searchModel);
}