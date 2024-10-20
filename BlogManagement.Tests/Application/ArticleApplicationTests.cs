using System.Text;
using _0_framework.Application;
using BlogManagement.Application;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace BlogManagement.Tests.Application;

public class ArticleApplicationTests
{
    private readonly Mock<IArticleRepository> _repositoryMock;
    private readonly Mock<IFileUploader> _fileUploaderMock;
    private readonly Mock<IFormFile> _fileMock;
    private readonly ArticleApplication _articleApplication;
    private readonly Mock<IArticleCategoryRepository> _articleCategoryRepository;

    public ArticleApplicationTests()
    {
        _repositoryMock = new Mock<IArticleRepository>();
        _fileUploaderMock = new Mock<IFileUploader>();
        _articleCategoryRepository = new Mock<IArticleCategoryRepository>();

        _fileMock = new Mock<IFormFile>();
        var content = "This is a dummy file";
        var fileName = "picture.jpg";
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
        _fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
        _fileMock.Setup(x => x.FileName).Returns(fileName);
        _fileMock.Setup(x => x.Length).Returns(ms.Length);

        _articleApplication = new ArticleApplication(_fileUploaderMock.Object, _repositoryMock.Object, _articleCategoryRepository.Object);
        

    }

    // [Fact]
    // public void Should_Create_Article_Successfully()
    // {
    //     
    //     // Create a dummy ArticleCategory
    //     var existingCategory = new ArticleCategory("Existing Category", "existing_picture.jpg",
    //         "Existing Alt", "Existing Title", "Existing Description", 1, "existing slug", "existing,keywords",
    //         "Existing Meta", "existing-slug");
    //
    //     _articleCategoryRepository.Setup(x => x.Get(command.Id)).Returns(existingCategory);
    //     
    //     
    //     // Arrange
    //     var categoryId = 1; // Assuming the category exists, you might need to create it in a real test setup
    //     var persianPublishDate = "1401/01/01";
    //     var command = new CreateArticle
    //     {
    //         Title = "Test Article",
    //         ShortDescription = "Test Short Description",
    //         Description = "Test Description",
    //         Picture = null,
    //         PictureAlt = "Test Alt",
    //         PictureTitle = "Test Title",
    //         PublishDate = persianPublishDate,
    //         Keywords = "test,keywords",
    //         MetaDescription = "Test Meta",
    //         CanonicalAddress = "test-article",
    //         CategoryId = 1
    //     }
    //     // Act
    //     // Assert
    // }
}