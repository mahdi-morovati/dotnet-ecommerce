using System.ComponentModel.DataAnnotations;
using _0_framework.Application;
using _0_framework.Domain;
using Microsoft.AspNetCore.Http;

namespace BlogManagement.Application.Contracts.Article;

public class CreateArticle : EntityBase
{
    [MaxLength(500, ErrorMessage = ValidationMessages.MaxLength)]
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Title { get; set; }

    [MaxLength(500, ErrorMessage = ValidationMessages.MaxLength)]
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public IFormFile Picture { get; set; }

    [MaxLength(500, ErrorMessage = ValidationMessages.MaxLength)]
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PictureAlt { get; set; }

    [MaxLength(500, ErrorMessage = ValidationMessages.MaxLength)]
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PictureTitle { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [ValidPersianDateFormat(ErrorMessage = ValidationMessages.DateValidFormat)]
    public string PublishDate { get; set; }

    [MaxLength(500, ErrorMessage = ValidationMessages.MaxLength)]
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Slug { get; set; }

    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Keywords { get; set; }

    [MaxLength(150, ErrorMessage = ValidationMessages.MaxLength)]
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string MetaDescription { get; set; }

    [MaxLength(1000, ErrorMessage = ValidationMessages.MaxLength)]
    public string CanonicalAddress { get; set; }

    [Range(1, long.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
    public long CategoryId { get; set; }
}