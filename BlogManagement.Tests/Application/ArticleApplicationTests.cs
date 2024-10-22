using System.Linq.Expressions;
using System.Text;
using Xunit;
using Moq;
using BlogManagement.Application;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using _0_framework.Application;
using Microsoft.AspNetCore.Http;

namespace BlogManagement.Tests.Application;

public class ArticleApplicationTests
{
    private readonly Mock<IArticleRepository> _articleRepositoryMock;
    private readonly Mock<IArticleCategoryRepository> _articleCategoryRepositoryMock;
    private readonly Mock<IFileUploader> _fileUploaderMock;
    private readonly ArticleApplication _articleApplication;
    private readonly Mock<IFormFile> _fileMock;

    public ArticleApplicationTests()
    {
        _articleRepositoryMock = new Mock<IArticleRepository>();
        _articleCategoryRepositoryMock = new Mock<IArticleCategoryRepository>();
        _fileUploaderMock = new Mock<IFileUploader>();
        _articleApplication = new ArticleApplication(
            _fileUploaderMock.Object,
            _articleRepositoryMock.Object,
            _articleCategoryRepositoryMock.Object);
        
        _fileMock = new Mock<IFormFile>();
        var content = "This is a dummy file";
        var fileName = "picture.jpg";
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
        _fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
        _fileMock.Setup(x => x.FileName).Returns(fileName);
        _fileMock.Setup(x => x.Length).Returns(ms.Length);
    }

    [Fact]
    public void Create_Should_Return_Success_When_Article_Is_Created()
    {
        // Arrange
        var categoryId = 1; // فرض می‌کنیم دسته‌بندی با شناسه 1 وجود دارد
        var category = new ArticleCategory("Test Category", "test.jpg", "Test Alt", "Test Title", 
            "Test Description", 1, "test-category", "test,keywords", "Test Meta", "Test Canonical");

        var command = new CreateArticle
        {
            Title = "Test Article",
            ShortDescription = "Test Short Description",
            Description = "Test Description",
            Picture = _fileMock.Object,
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            PublishDate = "1402/01/01", // تاریخ در فرمت تقویم ایرانی
            Keywords = "test,keywords",
            MetaDescription = "Test Meta",
            Slug = "test-article", // اضافه کردن Slug
            CategoryId = categoryId // استفاده از شناسه واقعی
        };

        // Mocking methods
        _articleRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Article, bool>>>()))
            .Returns(false); // فرض شده که مقاله با همین عنوان وجود ندارد

        _fileUploaderMock.Setup(uploader => uploader.Upload(It.IsAny<IFormFile>(), It.IsAny<string>()))
            .Returns("uploaded-picture-name.jpg"); // Mock کردن آپلود فایل
        
        // موک کردن دسته‌بندی
        _articleCategoryRepositoryMock.Setup(repo => repo.Get(categoryId))
            .Returns(category); // شبیه‌سازی اینکه دسته‌بندی با این شناسه وجود دارد

        // Act
        var result = _articleApplication.Create(command);

        // Assert
        Assert.True(result.IsSuccedded); // بررسی موفقیت عملیات
        _articleRepositoryMock.Verify(repo => repo.Create(It.IsAny<Article>()), Times.Once); // بررسی ایجاد مقاله
        _articleRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once); // بررسی ذخیره تغییرات
        
        // Verify that the Upload method was called with the correct parameters
        _fileUploaderMock.Verify(x => x.Upload(command.Picture, It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Empty_On_Create()
    {
        // Arrange
        var categoryId = 1; // فرض می‌کنیم دسته‌بندی با شناسه 1 وجود دارد
        var command = new CreateArticle
        {
            Title = "", // عنوان خالی
            ShortDescription = "Test Short Description",
            Description = "Test Description",
            Picture = _fileMock.Object,
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            PublishDate = "1402/01/01",
            Keywords = "test,keywords",
            MetaDescription = "Test Meta",
            Slug = "test-article",
            CategoryId = categoryId // استفاده از شناسه واقعی
        };

        // Act
        var exception = Assert.Throws<ArgumentException>(() => _articleApplication.Create(command));
        
        // Assert
        Assert.Equal(ValidationMessages.CannotBeEmpty + " (Parameter 'title')", exception.Message);
    }

    [Fact]
    public void Create_Should_Fail_When_Article_Title_Already_Exists()
    {
        // Arrange
        var categoryId = 1; // فرض می‌کنیم دسته‌بندی با شناسه 1 وجود دارد
        var command = new CreateArticle
        {
            Title = "Duplicate Title",
            ShortDescription = "Test Short Description",
            Description = "Test Description",
            Picture = null,
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            PublishDate = "1402/01/01",
            Keywords = "test,keywords",
            MetaDescription = "Test Meta",
            Slug = "duplicate-title", // اضافه کردن Slug
            CategoryId = categoryId // استفاده از شناسه واقعی
        };

        // Mocking article repository to return true for Exists method
        _articleRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Article, bool>>>()))
            .Returns(true); // شبیه‌سازی اینکه عنوان وجود دارد

        // Act
        var result = _articleApplication.Create(command);

        // Assert
        Assert.False(result.IsSuccedded); // عملیات باید شکست بخورد
        Assert.Equal(ApplicationMessages.DuplicatedRecord, result.Message); // بررسی پیام خطا
        _articleRepositoryMock.Verify(repo => repo.Create(It.IsAny<Article>()), Times.Never); // نباید مقاله ایجاد شود
        _articleRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Never); // نباید تغییرات ذخیره شوند
    }

    
}
