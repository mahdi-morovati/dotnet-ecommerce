using System.Linq.Expressions;
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
    private readonly Mock<IArticleRepository> _articleRepositoryMock;
    private readonly Mock<IFileUploader> _fileUploaderMock;
    private readonly Mock<IArticleCategoryRepository> _articleCategoryRepositoryMock;
    private readonly ArticleApplication _articleApplication;
    private readonly Mock<IFormFile> _fileMock;

    public ArticleApplicationTests()
    {
        _articleRepositoryMock = new Mock<IArticleRepository>();
        _fileUploaderMock = new Mock<IFileUploader>();
        _articleCategoryRepositoryMock = new Mock<IArticleCategoryRepository>();

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

    [Theory]
    [InlineData("1402/13/01")] // Invalid month
    [InlineData("1402/01/32")] // Invalid day
    [InlineData("1402/01")]    // Incomplete date
    [InlineData("abc/01/01")]  // Non-numeric year
    [InlineData("abcd/01/01")] // Non-numeric year
    public void Create_Article_With_Invalid_PublishDate_Should_Throw_FormatException(string invalidPublishDate)
    {
        Dictionary<string, string> InvalidDates = new Dictionary<string, string>
        {
            { "1402/13/01", ValidationMessages.DateValidMonthRange },
            { "1402/01/32", ValidationMessages.DateValidateDayRange },
            { "1402/01", ValidationMessages.DateValidFormat },
            { "abc/01/01", ValidationMessages.DateValidFormat },
            { "abcd/01/01", ValidationMessages.DateIsYearMonthDayNumeric }
        };

        // Arrange
        var command = new CreateArticle
        {
            Title = "Valid Title",
            ShortDescription = "Valid Short Description",
            Description = "Valid Description",
            PublishDate = invalidPublishDate,
            Picture = null,
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            Keywords = "test,keywords",
            MetaDescription = "Test Meta",
            Slug = "test-article",
            CategoryId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => _articleApplication.Create(command));
        InvalidDates.TryGetValue(invalidPublishDate, out string expectedMessage);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Create_Article_Should_Succeed_When_Valid()
    {
        // Arrange
        var command = new CreateArticle
        {
            Title = "Valid Title",
            ShortDescription = "Valid Short Description",
            Description = "Valid Description",
            PublishDate = "1402/01/01",
            Picture = _fileMock.Object, // Assuming the picture is not needed for the test
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            Keywords = "test,keywords",
            MetaDescription = "Test Meta",
            Slug = "test-article",
            CategoryId = 1
        };

        _articleCategoryRepositoryMock.Setup(x => x.GetSlugBy(command.CategoryId)).Returns("category-slug");
        _fileUploaderMock.Setup(x => x.Upload(command.Picture, It.IsAny<string>())).Returns("uploaded_picture.jpg");
        _articleRepositoryMock.Setup(x => x.Exists(It.IsAny<Expression<Func<Article, bool>>>())).Returns(false);

        // Act
        var result = _articleApplication.Create(command);

        // Assert
        Assert.True(result.IsSuccedded);
        _articleRepositoryMock.Verify(x => x.Create(It.IsAny<Article>()), Times.Once);
        _articleRepositoryMock.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Create_Article_Should_Fail_When_Title_Exists()
    {
        // Arrange
        var command = new CreateArticle
        {
            Title = "Existing Title",
            ShortDescription = "Valid Short Description",
            Description = "Valid Description",
            PublishDate = "1402/01/01",
            Picture = null,
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            Keywords = "test,keywords",
            MetaDescription = "Test Meta",
            Slug = "test-article",
            CategoryId = 1
        };

        _articleRepositoryMock.Setup(x => x.Exists(It.IsAny<Expression<Func<Article, bool>>>())).Returns(true);

        // Act
        var result = _articleApplication.Create(command);

        // Assert
        Assert.False(result.IsSuccedded);
        Assert.Equal(ApplicationMessages.DuplicatedRecord, result.Message);
    }

    [Fact]
    public void Edit_Article_Should_Succeed_When_Valid()
    {
        // Arrange
        var command = new EditArticle
        {
            Id = 1,
            Title = "Updated Title",
            ShortDescription = "Updated Short Description",
            Description = "Updated Description",
            PublishDate = "1402/01/01",
            Picture = _fileMock.Object,
            PictureAlt = "Updated Alt",
            PictureTitle = "Updated Title",
            Keywords = "updated,keywords",
            MetaDescription = "Updated Meta",
            Slug = "updated-article",
            CategoryId = 1
        };

        var existingArticle = new Article("Existing Title", "Short", "Desc", "picture.jpg", "Alt", "Title", DateTime.Now, "existing-slug", "keywords", "meta", "address", 1);
        _articleRepositoryMock.Setup(x => x.Get(command.Id)).Returns(existingArticle);
        _articleCategoryRepositoryMock.Setup(x => x.GetSlugBy(command.CategoryId)).Returns("category-slug");
        _fileUploaderMock.Setup(x => x.Upload(command.Picture, It.IsAny<string>())).Returns("uploaded_picture.jpg");
        _articleRepositoryMock.Setup(x => x.Exists(It.IsAny<Expression<Func<Article, bool>>>())).Returns(false);

        // Act
        var result = _articleApplication.Edit(command);

        // Assert
        Assert.True(result.IsSuccedded);
        _articleRepositoryMock.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Edit_Article_Should_Fail_When_Article_Not_Found()
    {
        // Arrange
        var command = new EditArticle { Id = 999 }; // Non-existing ID
        _articleRepositoryMock.Setup(x => x.Get(command.Id)).Returns((Article)null);

        // Act
        var result = _articleApplication.Edit(command);

        // Assert
        Assert.False(result.IsSuccedded);
        Assert.Equal(ApplicationMessages.RecordNotFound, result.Message);
    }

    [Fact]
    public void Edit_Article_Should_Fail_When_Title_Exists()
    {
        // Arrange
        var command = new EditArticle
        {
            Id = 1,
            Title = "Existing Title",
            ShortDescription = "Short Description",
            Description = "Description",
            PublishDate = "1402/01/01",
            Picture = null,
            PictureAlt = "Alt",
            PictureTitle = "Title",
            Keywords = "keywords",
            MetaDescription = "Meta",
            Slug = "updated-article",
            CategoryId = 1
        };

        var existingArticle = new Article("Another Title", "Short", "Desc", "picture.jpg", "Alt", "Title", DateTime.Now, "existing-slug", "keywords", "meta", "address", 1);
        _articleRepositoryMock.Setup(x => x.Get(command.Id)).Returns(existingArticle);
        _articleRepositoryMock.Setup(x => x.Exists(It.IsAny<Expression<Func<Article, bool>>>())).Returns(false);

        // Act
        var result = _articleApplication.Edit(command);

        // Assert
        Assert.False(result.IsSuccedded);
        Assert.Equal(ApplicationMessages.DuplicatedRecord, result.Message);
    }

    [Fact]
    public void GetDetails_Should_Return_Null_When_Article_Not_Found()
    {
        // Arrange
        _articleRepositoryMock.Setup(x => x.GetDetails(999)).Returns((EditArticle)null);

        // Act
        var result = _articleApplication.GetDetails(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetDetails_Should_Return_Article_Details()
    {
        // Arrange
        var expectedArticle = new EditArticle
        {
            Id = 1,
            Title = "Article Title",
            ShortDescription = "Short Description",
            Description = "Description",
            PublishDate = "1402/01/01",
            PictureAlt = "Alt",
            PictureTitle = "Title",
            Keywords = "keywords",
            MetaDescription = "Meta",
            Slug = "updated-article",
            CategoryId = 1
        };

        _articleRepositoryMock.Setup(x => x.GetDetails(expectedArticle.Id)).Returns(expectedArticle);

        // Act
        var result = _articleApplication.GetDetails(expectedArticle.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedArticle.Title, result.Title);
    }

    [Fact]
    public void Search_Should_Return_Articles()
    {
        // Arrange
        var searchModel = new ArticleSearchModel { Title = "Search Title" };
        var articles = new List<ArticleViewModel>
        {
            new ArticleViewModel { Title = "Search Title" },
            new ArticleViewModel { Title = "Another Title" }
        };

        _articleRepositoryMock.Setup(x => x.Search(searchModel)).Returns(articles);

        // Act
        var result = _articleApplication.Search(searchModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}