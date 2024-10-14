using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _0_framework.Application;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;

    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file == null)
        {
            return ValidationResult.Success; // اگر فایلی وجود ندارد، خطا ندهید
        }

        if (file.Length > _maxFileSize)
        {
            return new ValidationResult(ErrorMessage ?? $"فایل حجیم تر از حد مجاز است");
        }

        return ValidationResult.Success;
    }
}
