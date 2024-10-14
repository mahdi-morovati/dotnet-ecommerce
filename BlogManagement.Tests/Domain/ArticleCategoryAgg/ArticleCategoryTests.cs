using _0_framework.Application;
using BlogManagement.Domain.ArticleCategoryAgg;
using Xunit;

namespace BlogManagement.Tests.Domain.ArticleCategoryAgg;

public class ArticleCategoryTests
{
    [Fact]
    public void Should_Create_ArticleCategory_With_Valid_Data()
    {
        // Arrange
        var name = "Test Category";
        var picture = "test-picture.jpg";
        var pictureAlt = "Picture Alt Text";
        var pictureTitle = "Picture Title Text";
        var description = "This is a test category description.";
        var showOrder = 1;
        var slug = "test-category";
        var keywords = "test, category";
        var metaDescription = "This is a meta description for the category.";
        var canonicalAddress = "http://example.com/test-category";

        // Act
        var category = new ArticleCategory(name, picture, pictureAlt,
            pictureTitle, description, showOrder, slug, keywords, metaDescription, canonicalAddress);

        // Assert
        Assert.Equal(name, category.Name);
        Assert.Equal(picture, category.Picture);
        Assert.Equal(pictureAlt, category.PictureAlt);
        Assert.Equal(pictureTitle, category.PictureTitle);
        Assert.Equal(description, category.Description);
        Assert.Equal(showOrder, category.ShowOrder);
        Assert.Equal(slug, category.Slug);
        Assert.Equal(keywords, category.Keywords);
        Assert.Equal(metaDescription, category.MetaDescription);
        Assert.Equal(canonicalAddress, category.CanonicalAddress);
    }

    [Fact]
    public void Should_Edit_ArticleCategory_With_Valid_Data()
    {
        // Arrange
        var category = new ArticleCategory("Initial Name",
            "initial-picture.jpg", "Initial Alt", "Initial Title",
            "Initial Description", 1, "initial-slug", "initial, keywords",
            "Initial meta description", "http://example.com/initial");

        // Act
        category.Edit("Updated Name", "updated-picture.jpg", "Updated Alt", "Updated Title",
            "Updated Description", 2, "updated-slug", "updated, keywords",
            "Updated meta description", "http://example.com/updated");

        // Assert
        Assert.Equal("Updated Name", category.Name);
        Assert.Equal("updated-picture.jpg", category.Picture);
        Assert.Equal("Updated Alt", category.PictureAlt);
        Assert.Equal("Updated Title", category.PictureTitle);
        Assert.Equal("Updated Description", category.Description);
        Assert.Equal(2, category.ShowOrder);
        Assert.Equal("updated-slug", category.Slug);
        Assert.Equal("updated, keywords", category.Keywords);
        Assert.Equal("Updated meta description", category.MetaDescription);
        Assert.Equal("http://example.com/updated", category.CanonicalAddress);
    }
    
    [Fact]
    public void Should_Not_Update_ArticleCategory_With_Invalid_Data()
    {
        // Arrange
        var category = new ArticleCategory("Initial Name", "initial.jpg", "Initial Alt", "Initial Title", 
            "Initial Description", 1, "initial-slug", "initial,keywords", "Initial Meta", "http://initial.com");
        
        // Assert
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            category.Edit("", "", "", "", "", 0, "", "", "", "");
        });

        // Additional assertions to verify the exception details if needed
        Assert.Equal(ValidationMessages.CannotBeEmpty + " (Parameter 'name')", exception.Message);
            
        Assert.Equal("initial.jpg", category.Picture); // Ensure that picture is not changed
        Assert.Equal("Initial Alt", category.PictureAlt); // Ensure that PictureAlt is not changed
        Assert.Equal("Initial Title", category.PictureTitle); // Ensure that PictureTitle is not changed
        Assert.Equal("Initial Description", category.Description); // Ensure that Description is not changed
        Assert.Equal(1, category.ShowOrder); // Ensure that ShowOrder is not changed
        Assert.Equal("initial-slug", category.Slug); // Ensure that Slug is not changed
        Assert.Equal("initial,keywords", category.Keywords); // Ensure that Keywords is not changed
        Assert.Equal("Initial Meta", category.MetaDescription); // Ensure that MetaDescription is not changed
        Assert.Equal("http://initial.com", category.CanonicalAddress); // Ensure that CanonicalAddress is not changed
    }

}