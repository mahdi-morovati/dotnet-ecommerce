using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace _0_framework.Application.ZarinPal
{
    public class VerificationResponse
    {
        public Data data { get; set; }
        
        [JsonConverter(typeof(SingleOrArrayConverter<Error>))]
        public List<Error> errors { get; set; } // استفاده از کانورتر

        public class Data
        {
            public int code { get; set; }
            public long ref_id { get; set; }
        }

        public class Error
        {
            public string message { get; set; } // پیام خطا
            public int code { get; set; } // کد خطا
            public List<string> validations { get; set; } // اعتبارسنجی‌ها (در صورت وجود)
        }
    }
}