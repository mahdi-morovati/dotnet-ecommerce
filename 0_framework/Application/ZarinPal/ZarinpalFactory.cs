using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace _0_framework.Application.ZarinPal;

public class ZarinPalFactory : IZarinPalFactory
    {
        private readonly IConfiguration _configuration;

        public string Prefix { get; set; }
        private string MerchantId { get;}
        private static readonly HttpClient httpClient = new HttpClient();

        public ZarinPalFactory(IConfiguration configuration)
        {
            _configuration = configuration;
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
            amount = amount.Replace(",", "");
            var finalAmount = int.Parse(amount);
            
            var body = new VerificationRequest
            {
                amount = finalAmount,
                merchant_id = MerchantId,
                authority = authority
            };
            string jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync($"https://{Prefix}.zarinpal.com/pg/v4/payment/verify.json", content);
            string responseBody = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonConvert.DeserializeObject<VerificationResponse>(responseBody);
            
            if (paymentResponse != null && paymentResponse.data != null)
            {
                Console.WriteLine("Status: " + paymentResponse.data.code);
                Console.WriteLine("RefID: " + paymentResponse.data.ref_id);
            }
            
            return paymentResponse;
        }
    }