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

    [Fact]
    public void Should_Throw_Exception_When_Title_Is_Empty()
    {
        // Arrange
        var title = "";
        var shortDescription = "Short description";
        var description = "Description";
        var picture = "picture.jpg";
        var pictureAlt = "Alt";
        var pictureTitle = "Title";
        var publishDate = DateTime.Now;
        var slug = "slug";
        var keywords = "keywords";
        var metaDescription = "meta";
        var canonicalAddress = "https://example.com";
        var categoryId = 1;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Article(title, shortDescription, description, picture, pictureAlt,
            pictureTitle,
            publishDate, slug, keywords, metaDescription, canonicalAddress, categoryId));
    }

    [Fact]
    public void Should_Edit_Article_With_New_Values()
    {
        // Arrange
        var article = new Article("Old Title", "Old short description", "Old description", "old.jpg", "old alt",
            "old title",
            DateTime.Now, "old-slug", "old keywords", "old meta", "https://old.com", 1);

        var newTitle = "New Title";
        var newShortDescription = "New short description";
        var newDescription = "New description";
        var newPicture = "new.jpg";
        var newPictureAlt = "New alt";
        var newPictureTitle = "New title";
        var newPublishDate = DateTime.Now;
        var newSlug = "new-slug";
        var newKeywords = "new keywords";
        var newMetaDescription = "new meta";
        var newCanonicalAddress = "https://new.com";
        var newCategoryId = 2;

        // Act
        article.Edit(newTitle, newShortDescription, newDescription, newPicture, newPictureAlt, newPictureTitle,
            newPublishDate, newSlug, newKeywords, newMetaDescription, newCanonicalAddress, newCategoryId);

        // Assert
        Assert.Equal(newTitle, article.Title);
        Assert.Equal(newShortDescription, article.ShortDescription);
        Assert.Equal(newDescription, article.Description);
        Assert.Equal(newPicture, article.Picture);
        Assert.Equal(newPictureAlt, article.PictureAlt);
        Assert.Equal(newPictureTitle, article.PictureTitle);
        Assert.Equal(newPublishDate, article.PublishDate);
        Assert.Equal(newSlug, article.Slug);
        Assert.Equal(newKeywords, article.Keywords);
        Assert.Equal(newMetaDescription, article.MetaDescription);
        Assert.Equal(newCanonicalAddress, article.CanonicalAddress);
        Assert.Equal(newCategoryId, article.CategoryId);
    }

    [Fact]
    public void Should_Not_Change_Picture_If_New_Picture_Is_Empty()
    {
        // Arrange
        var article = new Article("Title", "Short description", "Description", "picture.jpg", "Alt", "Title",
            DateTime.Now, "slug", "keywords", "meta", "https://example.com", 1);

        var newPicture = "";

        // Act
        article.Edit("New Title", "New short description", "New description", newPicture, "New Alt", "New Title",
            DateTime.Now, "new-slug", "new keywords", "new meta", "https://new.com", 2);

        // Assert
        Assert.Equal("picture.jpg", article.Picture);
    }
}