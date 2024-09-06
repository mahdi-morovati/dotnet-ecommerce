using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Domain.SlideAgg;
using ShopManagement.Infrastructure.EFCore.Mapping;

namespace ShopManagement.Infrastructure.EFCore;

public class ShopContext : DbContext
{
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductPicture> ProductPictures { get; set; }
    public DbSet<Slide> Slides { get; set; }
    public DbSet<Order> Orders { get; set; }

    public ShopContext(DbContextOptions<ShopContext> options): base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(ProductCategoryMapping).Assembly;
        // میاد کل این اسمبلی رو چک میکنه و هر کلاسی که جنسشون مساوی با ProductCategoryMapping باشه یعنی IEntityTypeConfiguration رو پیاده سازی کرده باشن رو خود به خود اپلای میکنه روی modelBuilder  
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        
         base.OnModelCreating(modelBuilder);
    }
}