using _0_framework.Application;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BlogManagement.Tests.Mapping;

public class ArticleMappingTests
{
    [Fact]
    public void Should_Save_Article_To_Database_With_Correct_Mapping()
    {
        // Arrange
        // Add setup code for creating an in-memory database context
        var options = new DbContextOptionsBuilder<BlogContext>()
            .UseInMemoryDatabase(databaseName: "BlogTestDb")
            .Options;
        using (var context = new BlogContext(options))
        {
            var article = new Article("Test Title", "Test Short Description", "Description", "test.jpg", "Test Alt",
                "Test Title",
                DateTime.Now, "Test Slug", "Test Keywords", "Test Meta Description", "http://test.com", 1);

            // Act
            // Add an article to the context and save changes
            context.Articles.Add(article);
            context.SaveChanges();

            // Assert
            // Assertions to verify the saved article properties
            var savedArticle = context.Articles.FirstOrDefault(a => a.Title == "Test Title");
            Assert.NotNull(savedArticle);
            Assert.Equal("Test Title", savedArticle.Title);
            Assert.Equal("Test Short Description", savedArticle.ShortDescription);
        }
    }

    [Fact]
    public void Should_Throw_Exception_When_Title_Is_Empty()
    {
        // Arrange
        void CreateArticleWithEmptyTitle() =>
            new Article("", "Test Short Description", "Description", "picture.jpg", "Alt", "Title",
                DateTime.Now, "slug", "keywords", "meta", "http://example.com", 1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(CreateArticleWithEmptyTitle);
        Assert.Equal(ValidationMessages.CannotBeEmpty + " (Parameter 'title')", exception.Message);
    }
}