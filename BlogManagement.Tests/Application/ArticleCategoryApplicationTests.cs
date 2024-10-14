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
    private readonly Mock<IFormFile> _fileMock;

    public ArticleCategoryApplicationTests()
    {
        _repositoryMock = new Mock<IArticleCategoryRepository>();
        _fileUploaderMock = new Mock<IFileUploader>();
        _articleCategoryApplication =
            new ArticleCategoryApplication(_repositoryMock.Object, _fileUploaderMock.Object);

        _fileMock = new Mock<IFormFile>();
        var content = "This is a dummy file";
        var fileName = "picture.jpg";
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
        _fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
        _fileMock.Setup(x => x.FileName).Returns(fileName);
        _fileMock.Setup(x => x.Length).Returns(ms.Length);
    }

    [Fact]
    public void Should_Create_ProductCategory_Successfully()
    {
        // Arrange
        var command = new CreateArticleCategory
        {
            Name = "Test Category",
            Description = "Test Description",
            Slug = "test-category",
            Picture = _fileMock.Object,
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            Keywords = "test,keywords",
            MetaDescription = "Test Meta",
            ShowOrder = 1
        };

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

    [Fact]
    public void Should_Edit_ProductCategory_Successfully()
    {
        // Arrange
        var command = new EditArticleCategory
        {
            Id = 1,
            Name = "Updated Category",
            Description = "Updated Description",
            Slug = "updated-category",
            Picture = _fileMock.Object,
            PictureAlt = "Updated Alt",
            PictureTitle = "Updated Title",
            Keywords = "updated,keywords",
            MetaDescription = "Updated Meta",
            ShowOrder = 2
        };

        var existingCategory = new ArticleCategory("Existing Category", "existing_picture.jpg",
            "Existing Alt", "Existing Title", "Existing Description", 1, "existing slug", "existing,keywords",
            "Existing Meta", "existing-slug");

        // تنظیم شبیه‌سازی برای بازیابی موجودیت
        _repositoryMock.Setup(x => x.Get(command.Id)).Returns(existingCategory);
        _repositoryMock.Setup(x => x.Exists(It.IsAny<Expression<Func<ArticleCategory, bool>>>()))
            .Returns(false);
        _fileUploaderMock.Setup(x => x.Upload(command.Picture, It.IsAny<string>()))
            .Returns("updated_uploaded_picture.jpg");

        // Act
        var result = _articleCategoryApplication.Edit(command);

        // Assert
        Assert.True(result.IsSuccedded);
        _repositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        Assert.Equal("Updated Category", existingCategory.Name);
        Assert.Equal("Updated Description", existingCategory.Description);

        // Verify that the Upload method was called with the correct parameters
        _fileUploaderMock.Verify(x => x.Upload(command.Picture, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Empty_On_Create()
    {
        // Arrange
        var command = new CreateArticleCategory
        {
            Name = "",
            Description = "Test Description",
            Slug = "test-category",
            Picture = _fileMock.Object,
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            Keywords = "test,keywords",
            MetaDescription = "Test Meta",
            ShowOrder = 1
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _articleCategoryApplication.Create(command));
        Assert.Equal(ValidationMessages.CannotBeEmpty + " (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Empty_On_Edit()
    {
        // Arrange
        var command = new EditArticleCategory
        {
            Id = 1,
            Name = "",
            Description = "Updated Description",
            Slug = "updated-category",
            Picture = _fileMock.Object,
            PictureAlt = "Updated Alt",
            PictureTitle = "Updated Title",
            Keywords = "updated,keywords",
            MetaDescription = "Updated Meta",
            ShowOrder = 2
        };

        // تنظیم شبیه‌سازی برای بازیابی موجودیت
        var existingCategory = new ArticleCategory("Existing Category", "existing_picture.jpg",
            "Existing Alt", "Existing Title", "Existing Description", 1, "existing slug", "existing,keywords",
            "Existing Meta", "existing-slug");
        _repositoryMock.Setup(x => x.Get(command.Id)).Returns(existingCategory);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _articleCategoryApplication.Edit(command));
        Assert.Equal(ValidationMessages.CannotBeEmpty + " (Parameter 'name')", exception.Message);
    }
}