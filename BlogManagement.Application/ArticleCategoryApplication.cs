using _0_framework.Application;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;

namespace BlogManagement.Application;

public class ArticleCategoryApplication : IArticleCategoryApplication
{
    private readonly IArticleCategoryRepository _articleCategoryRepository;
    private readonly IFileUploader _fileUploader;

    public ArticleCategoryApplication(IArticleCategoryRepository articleCategoryRepository, IFileUploader fileUploader)
    {
        _articleCategoryRepository = articleCategoryRepository;
        _fileUploader = fileUploader;
    }

    public OperationResult Create(CreateArticleCategory command)
    {
        var operation = new OperationResult();
        if (_articleCategoryRepository.Exists(x => x.Name == command.Name))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var slug = command.Slug.Slugify();
        var pictureName = _fileUploader.Upload(command.Picture, slug);
        var articleCategory = new ArticleCategory(command.Name, pictureName, command.PictureAlt, command.PictureTitle,
            command.Description, command.ShowOrder, slug, command.Keywords, command.MetaDescription,
            command.CanonicalAddress);

        _articleCategoryRepository.Create(articleCategory);
        _articleCategoryRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult Edit(EditArticleCategory command)
    {
        throw new NotImplementedException();
    }

    public EditArticleCategory GetDetails(long id)
    {
        throw new NotImplementedException();
    }

    public List<ArticleCategoryViewModel> GetArticleCategories()
    {
        throw new NotImplementedException();
    }

    public List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel)
    {
        throw new NotImplementedException();
    }
}