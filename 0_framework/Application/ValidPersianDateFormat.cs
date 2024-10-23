using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _0_framework.Application;

public class ValidPersianDateFormat : ValidationAttribute, IClientModelValidator
{

    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-date-valid-format", ErrorMessage);
    }

    public override bool IsValid(object value)
    {
        var persianDate = value as string;
        if (persianDate == null) return true; // Allow null values to be valid (if nullable)

        // Check if the input is null or empty
        if (string.IsNullOrWhiteSpace(persianDate))
        {
            return false;
        }

        // Validate the length and structure
        if (persianDate.Length != 10 || persianDate[4] != '/' || persianDate[7] != '/')
        {
            return false;
        }

        // Extract year, month, day parts
        var yearPart = persianDate.Substring(0, 4);
        var monthPart = persianDate.Substring(5, 2);
        var dayPart = persianDate.Substring(8, 2);

        // Validate that year, month, and day are numeric
        if (!int.TryParse(yearPart, out var year) || 
            !int.TryParse(monthPart, out var month) || 
            !int.TryParse(dayPart, out var day))
        {
            return false;
        }

        // Validate month range (1 to 12)
        if (month < 1 || month > 12)
        {
            return false;
        }

        // Validate day range (1 to 31)
        // Note: For a more accurate day validation, consider month-specific limits
        if (day < 1 || day > 31)
        {
            return false;
        }

        // Additional checks can be added for months with fewer than 31 days
        // (e.g., April, June, September, November should have max 30 days)
        // Also, consider February depending on whether it's a leap year or not.

        return true;
    }
}
