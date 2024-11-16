using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using BlogManagement.Infrastructure.EFCore.Mappings;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore;

public class BlogContext : DbContext
{
    public DbSet<ArticleCategory> ArticleCategories { get; set; }
    public DbSet<Article> Articles { get; set; }
    
    public BlogContext(DbContextOptions<BlogContext> options): base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(ArticleCategoryMapping).Assembly;
        // میاد کل این اسمبلی رو چک میکنه و هر کلاسی که جنسشون مساوی با ArticleCategoryMapping باشه یعنی IEntityTypeConfiguration رو پیاده سازی کرده باشن رو خود به خود اپلای میکنه روی modelBuilder  
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}