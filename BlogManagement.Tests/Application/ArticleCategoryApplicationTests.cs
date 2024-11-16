using System.ComponentModel.DataAnnotations;
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
    public void Should_Create_ArticleCategory_Successfully()
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
    public void Should_Edit_ArticleCategory_Successfully()
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
    
    [Fact]
    public void Should_Throw_Exception_When_Name_Exists()
    {
        // Arrange
        var command = new EditArticleCategory
        {
            Id = 1,
            Name = "",
            Description = "Exists Description",
            Slug = "category",
            Picture = _fileMock.Object,
            PictureAlt = "Alt",
            PictureTitle = "Title",
            Keywords = "keywords",
            MetaDescription = "Meta",
            ShowOrder = 2
        };

        // تنظیم شبیه‌سازی برای بازیابی موجودیت
        var existingCategory = new ArticleCategory("Existing Category", "existing_picture.jpg",
            "Existing Alt", "Existing Title", "Existing Description", 1, "existing slug", "existing,keywords",
            "Existing Meta", "existing-slug");
        _repositoryMock.Setup(x => x.Get(command.Id)).Returns(existingCategory);
        _repositoryMock.Setup(x => x.Exists(It.IsAny<Expression<Func<ArticleCategory, bool>>>())).Returns(false);
        
        // Act
        var result = _articleCategoryApplication.Edit(command);

        // Assert
        Assert.False(result.IsSuccedded);
        Assert.Equal(ApplicationMessages.DuplicatedRecord, result.Message);
    }

    [Fact]
    public void Should_Return_Error_When_File_Has_Invalid_Extension()
    {
        // Arrange
        var command = new CreateArticleCategory
        {
            Name = "Test Category",
            Description = "Test Description",
            Slug = "test-category",
            Picture = _fileMock.Object, // این فایل باید پسوند نامعتبر داشته باشد
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            Keywords = "test,keywords",
            MetaDescription = "Test Meta",
            ShowOrder = 1
        };

        // تغییر پسوند فایل به نامعتبر
        _fileMock.Setup(x => x.FileName).Returns("invalid_file.txt");

        // Act
        var validationContext = new ValidationContext(command);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(command, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.ErrorMessage == ValidationMessages.InvalidFileFormat);
    }

    [Fact]
    public void Should_Return_Error_When_File_Is_Too_Large()
    {
        // Arrange
        var command = new CreateArticleCategory
        {
            Name = "Test Category",
            Description = "Test Description",
            Slug = "test-category",
            Picture = CreateLargeFile(), // متدی که فایل بزرگ ایجاد می‌کند
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            Keywords = "test,keywords",
            MetaDescription = "Test Meta",
            ShowOrder = 1
        };

        // Act
        var result = _articleCategoryApplication.Create(command);
        
        // Act
        var validationContext = new ValidationContext(command);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(command, validationContext, validationResults, true);
        
        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.ErrorMessage == ValidationMessages.MaxFileSize);
    }

    private IFormFile CreateLargeFile()
    {
        var content = new string('a', 50 * 1024 * 1024 + 1); // فایل بزرگتر از 3MB
        var fileName = "large_image.jpg";
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
        fileMock.Setup(x => x.FileName).Returns(fileName);
        fileMock.Setup(x => x.Length).Returns(ms.Length);
        return fileMock.Object;
    }
    
    [Fact]
    public void Should_Return_Error_When_Name_Already_Exists_On_Create()
    {
        // Arrange
        var command = new CreateArticleCategory
        {
            Name = "Duplicate Category",
            Description = "Test Description",
            Slug = "duplicate-category",
            Picture = _fileMock.Object,
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            Keywords = "test,keywords",
            MetaDescription = "Test Meta",
            ShowOrder = 1
        };

        _repositoryMock.Setup(x => x.Exists(It.IsAny<Expression<Func<ArticleCategory, bool>>>()))
            .Returns(true);

        // Act
        var result = _articleCategoryApplication.Create(command);

        // Assert
        Assert.False(result.IsSuccedded);
        Assert.Equal(ApplicationMessages.DuplicatedRecord, result.Message);
    }

}