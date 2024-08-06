namespace ShopManagement.Application.Contracts.ProductCategory;

public interface IProductCategorApplication
{
    void Create(CreateProductCategory command);
    void Edit(EditProductCategory command);
    EditProductCategory GetDetails(long id);
    List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);
}