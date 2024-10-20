using _0_framework.Application;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;

namespace BlogManagement.Application;

public class ArticleApplication : IArticleApplication
{
    private readonly IFileUploader _fileUploader;
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleCategoryRepository _articleCategoryRepository;

    public ArticleApplication(IFileUploader fileUploader, IArticleRepository articleRepository,
        IArticleCategoryRepository articleCategoryRepository)
    {
        _fileUploader = fileUploader;
        _articleRepository = articleRepository;
        _articleCategoryRepository = articleCategoryRepository;
    }

    public OperationResult Create(CreateArticle command)
    {
        var operation = new OperationResult();
        if (_articleRepository.Exists(x => x.Title == command.Title))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var slug = command.Title.Slugify();
        var categorySlug = _articleCategoryRepository.GetSlugBy(command.CategoryId);
        var path = $"{categorySlug}/{slug}";
        var pictureName = _fileUploader.Upload(command.Picture, path);
        var publishDate = command.PublishDate.ToGeorgianDateTime();

        var article = new Article(command.Title, command.ShortDescription, command.Description, pictureName,
            command.PictureAlt,
            command.PictureTitle, publishDate, slug, command.Keywords, command.MetaDescription,
            command.CanonicalAddress, command.CategoryId);

        _articleRepository.Create(article);
        _articleRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult Edit(EditArticle command)
    {
        throw new NotImplementedException();
    }

    public EditArticle GetDetails(long id)
    {
        throw new NotImplementedException();
    }

    public List<ArticleViewModel> Search(ArticleSearchModel command)
    {
        throw new NotImplementedException();
    }
}