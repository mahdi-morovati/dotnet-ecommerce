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

        persianDate = Helper.NumberConverter.ConvertToEnglishNumbers(persianDate);

        if (string.IsNullOrWhiteSpace(persianDate) || persianDate.Length != 10 || persianDate[4] != '/' || persianDate[7] != '/')
            // return false;
            throw new FormatException(ValidationMessages.DateValidFormat);

        // Extract year, month, day parts
        var yearPart = persianDate.Substring(0, 4);
        var monthPart = persianDate.Substring(5, 2);
        var dayPart = persianDate.Substring(8, 2);

        // Validate that year, month, and day are numeric
        if (!int.TryParse(yearPart, out var year) || !int.TryParse(monthPart, out var month) || !int.TryParse(dayPart, out var day))
            throw new FormatException(ValidationMessages.DateIsYearMonthDayNumeric);

        if (month < 1 || month > 12)
            throw new FormatException(ValidationMessages.DateValidMonthRange);
        
        if (day < 1 || day > 31)
            throw new FormatException(ValidationMessages.DateValidateDayRange);
        
        if (new[] {7, 8, 9, 10, 11}.Contains(month) && day > 30)
            throw new FormatException(ValidationMessages.DateMaxDaysInSecondHalfOfYear);
        
        
        if (month == 12)
        {
            if (day > (IsLeapYear(year) ? 30 : 29))
                throw new FormatException(ValidationMessages.DateDaysInLastMonthOfLeapYear);
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
