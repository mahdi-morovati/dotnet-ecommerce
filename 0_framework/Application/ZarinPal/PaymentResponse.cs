namespace _0_framework.Application.ZarinPal
{
    public class PaymentResponse
    {
        public Data Data { get; set; } // شامل اطلاعات مربوط به پرداخت
        public List<string> Errors { get; set; } // شامل خطاها
    }

    public class Data
    {
        public string Authority { get; set; } // Authority دریافتی
        public int Fee { get; set; } // هزینه
        public string FeeType { get; set; } // نوع هزینه
        public int Code { get; set; } // کد موفقیت
        public string Message { get; set; } // پیام
    }
}