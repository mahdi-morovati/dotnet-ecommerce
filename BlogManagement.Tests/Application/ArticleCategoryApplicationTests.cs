using _0_framework.Application;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;
using Moq;
using Xunit;

namespace BlogManagement.Tests.Application;

public class ArticleCategoryApplicationTests
{
    private readonly Mock<IArticleCategoryRepository> _mockRepository;
    private readonly Mock<IFileUploader> _mockFileUploader;
    private readonly ArticleCategoryApplication _application;

    public ArticleCategoryApplicationTests()
    {
        _mockRepository = new Mock<IArticleCategoryRepository>();
        _mockFileUploader = new Mock<IFileUploader>();
        _application = new ArticleCategoryApplication(_mockRepository.Object, _mockFileUploader.Object);
    }
    
    [Fact]
    public void Should_Create_ArticleCategory_Successfully()
    {
        // Arrange
        var articleCategoryApplication = new Mock<IArticleCategoryApplication>();
        var fileUploader = new Mock<IFileUploader>();
        var articleCategoryRepository = new Mock<IArticleCategoryRepository>();
        
        // Act
        articleCategoryApplication.Object.Create(new CreateArticleCategory { Name = "Category 1", Slug = "category-1" });
        
        // Assert
        articleCategoryRepository.Verify(x => x.Create(It.Is<ArticleCategory>(c => c.Name == "Category 1" && c.Slug == "category-1")));
        articleCategoryRepository.Verify(x => x.SaveChanges(), Times.Once);
    }
}