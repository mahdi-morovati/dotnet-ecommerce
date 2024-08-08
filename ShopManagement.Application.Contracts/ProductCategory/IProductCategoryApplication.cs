using _0_framework.Application;

namespace ShopManagement.Application.Contracts.ProductCategory;

public interface IProductCategoryApplication
{
    OperationResult Create(CreateProductCategory command);
    OperationResult Edit(EditProductCategory command);
    EditProductCategory GetDetails(long id);
    List<ProductCategoryViewModel> GetProductCategories();
    List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);
}