using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace _0_framework.Application.ZarinPal;

public class ZarinPalFactory : IZarinPalFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ZarinPalFactory> _logger;

        public string Prefix { get; set; }
        private string MerchantId { get;}
        private static readonly HttpClient httpClient = new HttpClient();

        public ZarinPalFactory(IConfiguration configuration, ILogger<ZarinPalFactory> logger)
        {
            _configuration = configuration;
            _logger = logger;
            Prefix = _configuration.GetSection("payment")["method"];
            MerchantId= _configuration.GetSection("payment")["merchant"];
        }
        
        public static async Task<PaymentResponse> PostRequestAsync(string url, object body)
        {
            string jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(responseBody);
                return paymentResponse;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return null;
            }
        }

        public async Task<PaymentResponse> CreatePaymentRequest(string amount, string mobile, string email,
            string description, long orderId)
        {
            amount = amount.Replace(",", "");
            var finalAmount = int.Parse(amount);
            var siteUrl = _configuration.GetSection("payment")["siteUrl"];
    
            var body = new PaymentRequest
            {
                mobile = mobile,
                callback_url = $"{siteUrl}/Checkout?handler=CallBack&oId={orderId}",
                description = description,
                email = email,
                amount = finalAmount,
                merchant_id = MerchantId
            };
    
            var response = await PostRequestAsync($"https://{Prefix}.zarinpal.com/pg/v4/payment/request.json", body);
    
            // اطمینان حاصل کنید که اگر response نال است، خطای مناسب را مدیریت کنید.
            if (response == null || response.Errors.Any())
            {
                // مدیریت خطا
                return null;
            }

            return response;
        }


        public async Task<VerificationResponse> CreateVerificationRequest(string authority, string amount)
        {
            try
            {
                // پاکسازی و تبدیل مقدار amount
                amount = amount.Replace(",", "");
                var finalAmount = int.Parse(amount);

                // ساخت body برای درخواست
                var body = new VerificationRequest
                {
                    amount = finalAmount,
                    merchant_id = MerchantId,
                    authority = authority
                };

                // تبدیل به JSON و ایجاد درخواست
                string jsonBody = JsonConvert.SerializeObject(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // ارسال درخواست به سرور زرین‌پال
                HttpResponseMessage response = await httpClient.PostAsync($"https://{Prefix}.zarinpal.com/pg/v4/payment/verify.json", content);

                // بررسی موفقیت درخواست
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(ApplicationMessages.PaymentVerifyFailed + response.StatusCode);
                }

                // دریافت پاسخ به صورت JSON
                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentResponse = JsonConvert.DeserializeObject<VerificationResponse>(responseBody);

                // بررسی محتوای response
                if (paymentResponse != null && paymentResponse.data != null)
                {
                    return paymentResponse;
                }
                else if (paymentResponse.errors != null && paymentResponse.errors.Count > 0)
                {
                    // در صورت وجود خطا در پاسخ، پیام مناسب نمایش داده می‌شود
                    throw new Exception(ApplicationMessages.PaymentFailed + paymentResponse.errors[0].message);
                }

                throw new Exception(ApplicationMessages.InvalidResponseFromGateway);
            }
            catch (Exception ex)
            {
                // مدیریت خطا
                _logger.LogError(ex, ApplicationMessages.ErrorOccurredWhileVerifyingPayment);
                throw new Exception(ApplicationMessages.ErrorOccurredWhileVerifyingPayment);
            }
        }
        
        

    }