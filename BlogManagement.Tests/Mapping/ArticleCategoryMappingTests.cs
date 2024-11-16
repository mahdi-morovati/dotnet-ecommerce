using _0_framework.Application;
using BlogManagement.Domain.ArticleCategoryAgg;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BlogManagement.Tests.Mapping;

public class ArticleCategoryMappingTests
{
    [Fact]
    public void Should_Save_ArticleCategory_To_Database_With_Correct_Mapping()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BlogContext>()
            .UseInMemoryDatabase(databaseName: "BlogTestDb")
            .Options;

        using (var context = new BlogContext(options))
        {
            var category = new ArticleCategory("Test Name", "test.jpg", "Test Alt", "Test Title",
                "Test Description", 1, "test-slug", "test, keywords", "Test Meta", "http://test.com");

            // Act
            context.ArticleCategories.Add(category);
            context.SaveChanges();

            // Assert
            var savedCategory = context.ArticleCategories.FirstOrDefault(c => c.Name == "Test Name");
            Assert.NotNull(savedCategory);
            Assert.Equal("Test Name", savedCategory.Name);
            Assert.Equal("test.jpg", savedCategory.Picture);
        }
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Empty()
    {
        // Arrange
        void CreateArticleCategoryWithEmptyName() =>
            new ArticleCategory("", "picture.jpg", "Alt", "Title", "Description", 1, "slug", "keywords", "meta",
                "http://example.com");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(CreateArticleCategoryWithEmptyName);
        Assert.Equal(ValidationMessages.CannotBeEmpty + " (Parameter 'name')", exception.Message);
    }
}