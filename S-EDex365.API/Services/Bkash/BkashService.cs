using RestSharp;
using S_EDex365.API.Interfaces.Bkash;
using S_EDex365.API.Models.Bkash;
using System.Text.Json;

namespace S_EDex365.API.Services.Bkash
{
    public class BkashService : IBkashService
    {
        private readonly string baseUrl = "https://tokenized.sandbox.bka.sh/v1.2.0-beta";
        private readonly string username = "sandboxTokenizedUser02";
        private readonly string password = "sandboxTokenizedUser02@12345";
        private readonly string appKey = "4f6o0cjiki2rfm34kfdadl1eqq";
        private readonly string appSecret = "2is7hdktrekvrbljjh44ll3d9l1dtjo4pasmjvs5vl5qr3fug4b";
        public async Task<BkashCreatePaymentResponse> CreatePaymentAsync(string token, BkashCreatePaymentRequest model)
        {
            var client = new RestClient($"{baseUrl}/tokenized/checkout/create");

            // Correct RestSharp syntax for POST method
            var request = new RestRequest("", Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("authorization", token);
            request.AddHeader("x-app-key", appKey);
            request.AddJsonBody(model);

            var response = await client.ExecuteAsync(request);

            return JsonSerializer.Deserialize<BkashCreatePaymentResponse>(response.Content);
        }


        public async Task<BkashExecutePaymentResponse> ExecutePaymentAsync(string token, string paymentId)
        {
            var client = new RestClient($"{baseUrl}/tokenized/checkout/execute");

            // Specify URL and method
            var request = new RestRequest("", Method.Post); // "" means no additional path segment

            // Add headers
            request.AddHeader("authorization", token);
            request.AddHeader("x-app-key", appKey);
            request.AddHeader("Content-Type", "application/json");

            // Add JSON body
            request.AddJsonBody(new { paymentID = paymentId });

            // Execute the request
            var response = await client.ExecuteAsync(request);

            // Deserialize the JSON response
            return JsonSerializer.Deserialize<BkashExecutePaymentResponse>(response.Content);
        }


        public async Task<string> GetTokenAsync()
        {
            var client = new RestClient($"{baseUrl}/tokenized/checkout/token/grant");

            // Correct usage of RestRequest with POST method
            var request = new RestRequest("", Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("username", username);
            request.AddHeader("password", password);

            request.AddJsonBody(new
            {
                app_key = appKey,
                app_secret = appSecret
            });

            var response = await client.ExecuteAsync(request);

            var token = JsonSerializer.Deserialize<BkashTokenResponse>(response.Content);

            return token?.id_token;
        }

    }
}
