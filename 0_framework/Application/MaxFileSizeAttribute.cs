using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _0_framework.Application;

public class MaxFileSizeAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly int _maxFileSize;

    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    public override bool IsValid(object value)
    {
        var file = value as IFormFile;
        if (file == null) return true;

        var eq = file.Length <= _maxFileSize;
        return eq;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-file-maxFileSize", ErrorMessage);
    }
}