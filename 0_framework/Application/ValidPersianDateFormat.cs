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

        if (string.IsNullOrWhiteSpace(persianDate) || persianDate.Length != 10 || persianDate[4] != '/' || persianDate[7] != '/')
            return false;

        // Extract year, month, day parts
        var yearPart = persianDate.Substring(0, 4);
        var monthPart = persianDate.Substring(5, 2);
        var dayPart = persianDate.Substring(8, 2);

        // Validate that year, month, and day are numeric
        if (!int.TryParse(yearPart, out var year) || !int.TryParse(monthPart, out var month) || !int.TryParse(dayPart, out var day))
            return false;

        if (month < 1 || month > 12 || day < 1 || day > 31 || (month == 2 && day > 29) || (new[] {4, 6, 9, 11}.Contains(month) && day > 30))
            return false;
        
        if (month == 12)
        {
            if (day > (IsLeapYear(year) ? 29 : 28))
                return false;
        }


        // Additional checks can be added for months with fewer than 31 days
        // (e.g., April, June, September, November should have max 30 days)
        // Also, consider February depending on whether it's a leap year or not.

        return true;
    }
    
    private bool IsLeapYear(int year)
    {
        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }

}
