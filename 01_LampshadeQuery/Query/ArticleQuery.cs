using _0_framework.Application;
using _01_LampshadeQuery.Contracts.Article;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_LampshadeQuery.Query;

public class ArticleQuery : IArticleQuery
{
    private readonly BlogContext _context;

    public ArticleQuery(BlogContext context)
    {
        _context = context;
    }

    public List<ArticleQueryModel> LatestArticles()
    {
        return _context.Articles
            .Where(x => x.PublishDate <= DateTime.Now)
            .Select(x => new ArticleQueryModel
            {
                Title = x.Title,
                Slug = x.Slug,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription,
            }).ToList();
    }

    public ArticleQueryModel? GetArticleDetails(string slug)
    {
        var article = _context.Articles
            .Include(x => x.Category)
            .Where(x => x.PublishDate <= DateTime.Now)
            .Select(x => new ArticleQueryModel
            {
                Id = x.Id,
                Title = x.Title,
                CategoryName = x.Category.Name,
                CategorySlug = x.Category.Slug,
                Slug = x.Slug,
                CanonicalAddress = x.CanonicalAddress,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription,
            }).FirstOrDefault(x => x.Slug == slug);
        if (!string.IsNullOrWhiteSpace(article?.Keywords))
            article.KeywordList = article.Keywords.Split(",").ToList();

        return article;
    }
}