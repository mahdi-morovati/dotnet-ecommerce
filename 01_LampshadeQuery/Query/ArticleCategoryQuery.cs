using _0_framework.Application;
using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_LampshadeQuery.Query;

public class ArticleCategoryQuery : IArticleCategoryQuery
{
    private readonly BlogContext _dbContext;

    public ArticleCategoryQuery(BlogContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<ArticleCategoryQueryModel> GetArticleCategories()
    {
        return _dbContext.ArticleCategories
            .Include(x => x.Articles)
            .Select(x => new ArticleCategoryQueryModel
            {
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug,
                Description = x.Description,
                ArticlesCount = x.Articles.Count
            }).ToList();
    }

    public ArticleCategoryQueryModel? GetArticleCategory(string slug)
    {
        var articleCategory = _dbContext.ArticleCategories
            .AsNoTracking() 
            .Include(x => x.Articles)
            .Where(x => x.Slug == slug)
            .Select(x => new ArticleCategoryQueryModel
            {
                Slug = x.Slug,
                Name = x.Name,
                Description = x.Description,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                CanonicalAddress = x.CanonicalAddress,
                ArticlesCount = x.Articles.Count,
                Articles = MapArticles(x.Articles)
            })
           .FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(articleCategory?.Keywords))
            articleCategory.KeywordList = articleCategory.Keywords.Split(",").ToList();

        return articleCategory;
    }

    private static List<ArticleQueryModel> MapArticles(List<Article> articles)
    {
        return articles.Select(x => new ArticleQueryModel
        {
            Slug = x.Slug,
            ShortDescription = x.ShortDescription,
            Title = x.Title,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            PublishDate = x.PublishDate.ToFarsi(),
        }).ToList();
    }
}