using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace _0_framework.Application.ZarinPal;

public class ZarinPalFactory : IZarinPalFactory
{
    private readonly IConfiguration _configuration;

    public string Prefix { get; set; }
    private string MerchantId { get; }

    public ZarinPalFactory(IConfiguration configuration)
    {
        _configuration = configuration;
        Prefix = _configuration.GetSection("payment")["method"];
        MerchantId = _configuration.GetSection("payment")["merchant"];
    }

    public PaymentResponse CreatePaymentRequest(string amount, string mobile, string email, string description,
        long orderId)
    {
        amount = amount.Replace(",", "");
        var finalAmount = int.Parse(amount);
        var siteUrl = _configuration.GetSection("payment")["siteUrl"];

        var client = new RestClient($"https://{Prefix}.zarinpal.com/");
        var request = new RestRequest("pg/rest/WebGate/PaymentRequest.json", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        var body = new PaymentRequest
        {
            Mobile = mobile,
            CallbackURL = $"{siteUrl}/Checkout?handler=CallBack&oId={orderId}",
            Description = description,
            Email = email,
            Amount = finalAmount,
            MerchantID = MerchantId
        };
        request.AddJsonBody(body);
        var response = client.Execute(request);
        
        Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body));
        Console.WriteLine("Response Content: " + response.Content);

        // var jsonSerializer = new JsonSerializer();
        if (response.IsSuccessful)
        {
            var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(response.Content);
            return paymentResponse;
        }

        return null;
        
        // return jsonSerializer.Deserialize<PaymentResponse>(response);
    }

    public VerificationResponse CreateVerificationRequest(string authority, string amount)
    {
        var client = new RestClient($"https://{Prefix}.zarinpal.com/");
        var request = new RestRequest("pg/rest/WebGate/PaymentVerification.json", Method.Post);
        
        
        request.AddHeader("Content-Type", "application/json");

        amount = amount.Replace(",", "");
        var finalAmount = int.Parse(amount);

        request.AddJsonBody(new VerificationRequest
        {
            Amount = finalAmount,
            MerchantID = MerchantId,
            Authority = authority
        });
        var response = client.Execute(request);

        if (response.IsSuccessful)
        {
            var verificationResponse = JsonConvert.DeserializeObject<VerificationResponse>(response.Content);
            return verificationResponse;
        }

        return null;

    }
}