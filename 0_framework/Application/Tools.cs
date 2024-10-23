using System.Globalization;

namespace _0_framework.Application
{
    public static class Tools
    {
        public static string[] MonthNames =
            {"فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"};

        public static string[] DayNames = {"شنبه", "یکشنبه", "دو شنبه", "سه شنبه", "چهار شنبه", "پنج شنبه", "جمعه"};
        public static string[] DayNamesG = {"یکشنبه", "دو شنبه", "سه شنبه", "چهار شنبه", "پنج شنبه", "جمعه", "شنبه"};


        public static string ToFarsi(this DateTime? date)
        {
            try
            {
                if (date != null) return date.Value.ToFarsi();
            }
            catch (Exception)
            {
                return "";
            }

            return "";
        }

        public static string ToFarsi(this DateTime date)
        {
            if (date == new DateTime()) return "";
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
        }
        
        public static string ToDiscountFormat(this DateTime date)
        {
            if (date == new DateTime()) return "";
            return $"{date.Year}/{date.Month}/{date.Day}";
        }

        public static string GetTime(this DateTime date)
        {
            return $"_{date.Hour:00}_{date.Minute:00}_{date.Second:00}";
        }

        public static string ToFarsiFull(this DateTime date)
        {
            var pc = new PersianCalendar();
            return
                $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00} {date.Hour:00}:{date.Minute:00}:{date.Second:00}";
        }

        private static readonly string[] Pn = {"۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"};
        private static readonly string[] En = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};

        public static string ToEnglishNumber(this string strNum)
        {
            var cash = strNum;
            for (var i = 0; i < 10; i++)
                cash = cash.Replace(Pn[i], En[i]);
            return cash;
        }

        public static string ToPersianNumber(this int intNum)
        {
            var chash = intNum.ToString();
            for (var i = 0; i < 10; i++)
                chash = chash.Replace(En[i], Pn[i]);
            return chash;
        }

        public static DateTime? FromFarsiDate(this string InDate)
        {
            if (string.IsNullOrEmpty(InDate))
                return null;

            var spited = InDate.Split('/');
            if (spited.Length < 3)
                return null;

            if (!int.TryParse(spited[0].ToEnglishNumber(), out var year))
                return null;

            if (!int.TryParse(spited[1].ToEnglishNumber(), out var month))
                return null;

            if (!int.TryParse(spited[2].ToEnglishNumber(), out var day))
                return null;
            var c = new PersianCalendar();
            return c.ToDateTime(year, month, day, 0, 0, 0, 0);
        }


        public static DateTime ToGeorgianDateTime(this string persianDate)
        {
            persianDate = persianDate.ToEnglishNumber();
    
            // Validate the length and structure
            if (persianDate.Length != 10 || persianDate[4] != '/' || persianDate[7] != '/')
            {
                throw new FormatException("تاریخ باید با فرمت YYYY/MM/DD باشد");
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
                throw new FormatException("تاریخ باید با فرمت YYYY/MM/DD باشد");
            }

            // Validate month range (1 to 12)
            if (month < 1 || month > 12)
            {
                throw new FormatException("ماه باید بین 1 و 12 باشد");
            }

            // Validate day range (1 to 31)
            if (day < 1 || day > 31)
            {
                throw new FormatException("روز باید بین 1 و 31 باشد");
            }

            // Additional day validation based on month
            if ((month == 4 || month == 6 || month == 9 || month == 11) && day > 30)
            {
                throw new FormatException($"ماه {month} فقط 30 روز دارد");
            }

            // Check for February (assume Persian month count)
            if (month == 2)
            {
                if (day > 29)
                {
                    throw new FormatException("فوریه حداکثر 29 روز دارد");
                }
            }

            // If everything is valid, create the DateTime
            return new DateTime(year, month, day, new PersianCalendar());
        }

        public static string ToMoney(this double myMoney)
        {
            return myMoney.ToString("N0", CultureInfo.CreateSpecificCulture("fa-ir"));
        }

        public static string ToFileName(this DateTime date)
        {
            return $"{date.Year:0000}-{date.Month:00}-{date.Day:00}-{date.Hour:00}-{date.Minute:00}-{date.Second:00}";
        }
    }
}