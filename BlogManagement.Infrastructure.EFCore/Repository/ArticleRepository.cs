using _0_framework.Application;
using _0_framework.Infrastructure;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore.Repository;

public class ArticleRepository : RepositoryBase<long, Article>, IArticleRepository
{
    private readonly BlogContext _context;

    public ArticleRepository(BlogContext context) : base(context)
    {
        _context = context;
    }

    public EditArticle? GetDetails(long id)
    {
        return _context.Articles.Select(x => new EditArticle
        {
            Id = x.Id,
            Title = x.Title,
            CategoryId = x.CategoryId,
            PublishDate = x.PublishDate.ToFarsi(),
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Description = x.Description,
            ShortDescription = x.ShortDescription,
            Slug = x.Slug,
            MetaDescription = x.MetaDescription,
            Keywords = x.Keywords,
            CanonicalAddress = x.CanonicalAddress,
        }).FirstOrDefault(x => x.Id == id);
    }

    public Article GetWithCategory(long id)
    {
        return _context.Articles.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
    }

    public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
    {
        var query = _context.Articles.Select(x => new ArticleViewModel
        {
            Id = x.Id,
            Title = x.Title,
            PublishDate = x.PublishDate.ToFarsi(),
            CategoryId = x.CategoryId,
            Category = x.Category.Name,
            ShortDescription = x.ShortDescription.Substring(0, Math.Min(x.ShortDescription.Length, 50)) + " ...",
        });
        if (!string.IsNullOrWhiteSpace(searchModel.Title))
        {
            query = query.Where(x => x.Title.Contains(searchModel.Title));
        }

        if (searchModel.CategoryId > 0)
        {
            query = query.Where(x => x.CategoryId == searchModel.CategoryId);
        }

        return query.OrderByDescending(x => x.Id).ToList();
    }
}