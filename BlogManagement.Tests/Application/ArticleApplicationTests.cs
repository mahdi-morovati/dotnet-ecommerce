using _0_framework.Application;
using Xunit;
using BlogManagement.Application;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using Moq;

public class ArticleApplicationTests
{
    private readonly Mock<IArticleRepository> _articleRepositoryMock;
    private readonly Mock<IFileUploader> _fileUploaderMock;
    private readonly Mock<IArticleCategoryRepository> _articleCategoryRepositoryMock;
    private readonly ArticleApplication _articleApplication;

    public ArticleApplicationTests()
    {
        _articleRepositoryMock = new Mock<IArticleRepository>();
        _fileUploaderMock = new Mock<IFileUploader>();
        _articleCategoryRepositoryMock = new Mock<IArticleCategoryRepository>();

        _articleApplication = new ArticleApplication(
            _fileUploaderMock.Object,
            _articleRepositoryMock.Object,
            _articleCategoryRepositoryMock.Object);
    }

    [Theory]
    [InlineData("1402/13/01")] // Invalid month
    [InlineData("1402/01/32")] // Invalid day
    [InlineData("1402/01")]    // Incomplete date
    [InlineData("abc/01/01")]  // Non-numeric year
    [InlineData("abcd/01/01")]  // Non-numeric year
    public void Create_Article_With_Invalid_PublishDate_Should_Throw_FormatException_For_Multiple_Formats(string invalidPublishDate)
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
            Picture = null, // Set this to a mock or null if not used in the test
            PictureAlt = "Test Alt",
            PictureTitle = "Test Title",
            Keywords = "test,keywords",
            MetaDescription = "Test Meta",
            Slug = "test-article",
            CategoryId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => _articleApplication.Create(command));
        // Assert.Equal(InvalidDates.TryGetValue(invalidPublishDate, out string result), exception.Message);
        InvalidDates.TryGetValue(invalidPublishDate, out string expectedMessage);
        Assert.Equal(expectedMessage, exception.Message);
    }
}
