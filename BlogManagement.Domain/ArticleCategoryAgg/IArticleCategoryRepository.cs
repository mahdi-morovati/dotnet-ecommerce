using BlogManagement.Application.Contracts.ArticleCategory;

namespace BlogManagement.Domain.ArticleCategoryAgg;

public interface IArticleCategoryRepository
{
    string? GetSlugBy(long id);
    EditArticleCategory? GetDetails(long id);
    List<ArticleCategoryViewModel> GetArticleCategories();
    List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel);
}