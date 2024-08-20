using _0_framework.Application;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Application;

public class ProductPictureApplication : IProductPictureApplication
{
    private readonly IFileUploader _fileUploader;
    private readonly IProductRepository _productRepository;
    private readonly IProductPictureRepository _productPictureRepository;

    public ProductPictureApplication(IProductPictureRepository productPictureRepository, IProductRepository productRepository, IFileUploader fileUploader)
    {
        _productPictureRepository = productPictureRepository;
        _productRepository = productRepository;
        _fileUploader = fileUploader;
    }

    public OperationResult Create(CreateProductPicture command)
    {
        var operation = new OperationResult();

        var product = _productRepository.GetWithCategory(command.ProductId);
        var path = $"{product.Category.Slug}/{product.Slug}";
        var picturePath = _fileUploader.Upload(command.Picture, path);

        var productPicture =
            new ProductPicture(command.ProductId, picturePath, command.PictureAlt, command.PictureTitle);
        _productPictureRepository.Create(productPicture);
        _productPictureRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult Edit(EditProductPicture command)
    {
        var operation = new OperationResult();
        var productPicture = _productPictureRepository.GetWithProductAndCategory(command.Id);
        if (productPicture == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        var path = $"{productPicture.Product.Category.Slug}/{productPicture.Product.Slug}";
        var picturePath = _fileUploader.Upload(command.Picture, path);

        productPicture.Edit(command.ProductId, picturePath, command.PictureAlt, command.PictureTitle);
        _productPictureRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult Remove(long id)
    {
        var operation = new OperationResult();
        var productPicture = _productPictureRepository.Get(id);
        if (productPicture == null)
        {
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        productPicture.Removed();
        _productPictureRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult Restore(long id)
    {
        var operation = new OperationResult();
        var productPicture = _productPictureRepository.Get(id);
        if (productPicture == null)
        {
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        productPicture.Restore();
        _productPictureRepository.SaveChanges();
        return operation.Succedded();
    }

    public EditProductPicture GetDetails(long id)
    {
        return _productPictureRepository.GetDetails(id);
    }

    public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
    {
        return _productPictureRepository.Search(searchModel);
    }
}