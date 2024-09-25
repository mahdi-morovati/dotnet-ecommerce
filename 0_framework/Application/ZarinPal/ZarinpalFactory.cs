using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

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
            // تبدیل body به رشته JSON با استفاده از JsonConvert
            string jsonBody = JsonConvert.SerializeObject(body);

            // ایجاد محتوا به صورت JSON
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // ارسال درخواست POST
            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            Console.WriteLine(response);

            // اطمینان از اینکه پاسخ موفقیت‌آمیز بوده
            response.EnsureSuccessStatusCode();

            // خواندن محتوای پاسخ به صورت رشته
            string responseBody = await response.Content.ReadAsStringAsync();

            // دی‌سریالایز کردن پاسخ به PaymentResponse
            var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(responseBody);

            return paymentResponse;
        }

        public async Task<PaymentResponse> CreatePaymentRequest(string amount, string mobile, string email,
            string description,
            long orderId)
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
            

            return response;
        }

        public VerificationResponse CreateVerificationRequest(string authority, string amount)
        {
            return null;
            // var client = new RestClient($"https://{Prefix}.zarinpal.com/pg/v4/payment/verify.json");
            // // var request = new RestRequest(Method.Po);
            // request.AddHeader("Content-Type", "application/json");
            //
            // amount = amount.Replace(",", "");
            // var finalAmount = int.Parse(amount);
            //
            // request.AddJsonBody(new VerificationRequest
            // {
            //     Amount = finalAmount,
            //     MerchantID = MerchantId,
            //     Authority = authority
            // });
            // var response = client.Execute(request);
            // var jsonSerializer = new JsonSerializer();
            // // return jsonSerializer.Deserialize<VerificationResponse>(response);
        }
    }