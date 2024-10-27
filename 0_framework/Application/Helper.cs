namespace _0_framework.Application;

public class Helper
{
    public static class NumberConverter
    {
        public static string ConvertToEnglishNumbers(object numbers)
        {
            if (numbers == null) return null;

            string numberString = numbers.ToString();
            if (string.IsNullOrWhiteSpace(numberString)) return numberString;

            // تعریف اعداد فارسی
            var persianNumbers = new Dictionary<char, char>
            {
                { '۰', '0' },
                { '۱', '1' },
                { '۲', '2' },
                { '۳', '3' },
                { '۴', '4' },
                { '۵', '5' },
                { '۶', '6' },
                { '۷', '7' },
                { '۸', '8' },
                { '۹', '9' },
                { '٠', '0' },
                { '١', '1' },
                { '٢', '2' },
                { '٣', '3' },
                { '٤', '4' },
                { '٥', '5' },
                { '٦', '6' },
                { '٧', '7' },
                { '٨', '8' },
                { '٩', '9' }
            };

            // جایگزینی اعداد فارسی با انگلیسی
            foreach (var persianNumber in persianNumbers)
            {
                numberString = numberString.Replace(persianNumber.Key.ToString(), persianNumber.Value.ToString());
            }

            return numberString;
        }
    }
}