using BlogManagement.Domain.ArticleAgg;
using Xunit;

namespace BlogManagement.Tests.Domain.ArticleAgg;

public class ArticleTests
{
    [Fact]
    public void Should_Create_Article_With_Valid_Data()
    {
        // Arrange
        var title = "Test Title";
        var shortDescription = "This is a short description.";
        var description = "This is a long description.";
        var picture = "test-picture.jpg";
        var pictureAlt = "Picture Alt Text";
        var pictureTitle = "Picture Title Text";
        var publishDate = DateTime.Now;
        var slug = "test-title";
        var keywords = "test, article";
        var metaDescription = "This is a meta description.";
        var canonicalAddress = "http://example.com/test-title";
        var categoryId = 1;

        // Act
        var article = new Article(title, shortDescription, description, picture, pictureAlt, pictureTitle, publishDate,
            slug, keywords, metaDescription, canonicalAddress, categoryId);

        // Assert
        Assert.Equal(title, article.Title);
        Assert.Equal(shortDescription, article.ShortDescription);
        Assert.Equal(description, article.Description);
        Assert.Equal(picture, article.Picture);
        Assert.Equal(pictureAlt, article.PictureAlt);
        Assert.Equal(pictureTitle, article.PictureTitle);
        Assert.Equal(publishDate, article.PublishDate);
        Assert.Equal(slug, article.Slug);
        Assert.Equal(keywords, article.Keywords);
        Assert.Equal(metaDescription, article.MetaDescription);
        Assert.Equal(canonicalAddress, article.CanonicalAddress);
        Assert.Equal(categoryId, article.CategoryId);
    }
}