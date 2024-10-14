using System.Linq.Expressions;
using System.Text;
using _0_framework.Application;
using BlogManagement.Application;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace BlogManagement.Tests.Application;

public class ArticleCategoryApplicationTests
{
    private readonly Mock<IArticleCategoryRepository> _repositoryMock;
    private readonly Mock<IFileUploader> _fileUploaderMock;
    private readonly ArticleCategoryApplication _articleCategoryApplication;

    public ArticleCategoryApplicationTests()
    {
        _repositoryMock = new Mock<IArticleCategoryRepository>();
        _fileUploaderMock = new Mock<IFileUploader>();
        _articleCategoryApplication = new ArticleCategoryApplication(_repositoryMock.Object, _fileUploaderMock.Object);

    }
    
    [Fact]
    public void Should_Create_ProductCategory_Successfully()
    {
        var fileMock = new Mock<IFormFile>();
        var content = "This is a dummy file";
        var fileName = "picture.jpg";
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
        fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
        fileMock.Setup(x => x.FileName).Returns(fileName);
        fileMock.Setup(x => x.Length).Returns(ms.Length);
        
        // Arrange
        var command = new CreateArticleCategory
        {
            Name = "Test Category",
            Description = "Test Description",
            Slug = "test-category",
            Picture = fileMock.Object,
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            Keywords = "test,keywords",
            MetaDescription = "Test Meta"
        };

        // _repositoryMock.Setup(x => x.Exists(It.Is<Func<ArticleCategory, bool>>(f => true))).Returns(false);
        _repositoryMock.Setup(x => x.Exists(It.IsAny<Expression<Func<ArticleCategory, bool>>>()))
            .Returns(false);
        _fileUploaderMock.Setup(x => x.Upload(command.Picture, It.IsAny<string>())).Returns("uploaded_picture.jpg");
        
        // Act
        var result = _articleCategoryApplication.Create(command);

        // Assert
        Assert.True(result.IsSuccedded);
        _repositoryMock.Verify(x => x.Create(It.IsAny<ArticleCategory>()), Times.Once);
        _repositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        
        // Verify that the Upload method was called with the correct parameters
        _fileUploaderMock.Verify(x => x.Upload(command.Picture, It.IsAny<string>()), Times.Once);

    }
}