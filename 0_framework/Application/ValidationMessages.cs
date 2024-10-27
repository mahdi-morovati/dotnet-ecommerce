namespace _0_framework.Application;

/**
 * create static fields for form validation error messages
 */
public static class ValidationMessages
{
    public const string IsRequired = "این مقدار نمی تواند خالی باشد";
    public const string MaxFileSize = "فایل حجیم تر از حد مجاز است";
    public const string InvalidFileFormat = "فرمت فایل مجاز نیست";
    public const string MaxLength = "مقدار وارد شده بیش از طول مجاز است";
    public const string CannotBeEmpty = "پارامتر نمی‌تواند خالی باشد";
    public const string DateCannotBeEmpty = "تاریخ نمی‌تواند خالی باشد";
    public const string DateValidFormat = "تاریخ باید با فرمت YYYY/MM/DD باشد";
    public const string DateIsYearMonthDayNumeric = "سال و ماه و روز باید عدد باشند";
    public const string DateValidMonthRange = "ماه باید بین 1 تا 12 باشد";
    public const string DateValidateDayRange = "مقدار روز نباید کمتر از 1 و بیشتر از 31 باشد";
    public const string DateMaxDaysInSecondHalfOfYear = "در نیمه دوم سال مقدار روز نباید بیشتر از 30 باشد";
    public const string DateDaysInLastMonthOfLeapYear = "تعداد روز ماه آخر در سال کبیسه تعداد روز 30 در غیر این صورت باید 29 باشد";
}