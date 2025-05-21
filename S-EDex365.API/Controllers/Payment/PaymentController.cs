using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using S_EDex365.API.Models.Payment;
using System.Net;
using System.Security.Claims;
using System.Text;
using S_EDex365.API.Interfaces;

namespace S_EDex365.API.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public string MerchantId = "683002007104225";

        private DateTime DateTime = DateTime.Now.AddMinutes(-5);
        public string RequestDateTime = DateTime.Now.ToString("yyyyMMddHHmmss"); //{// Format should be 20200827134915

        //Generate Random Number
        static Random r = new Random();
        public static int RandomNumber = r.Next(100000000, 999999999); //Randam Number should be less than 20 char

        //Initialize API URL
        public static string InitializeAPI = "http://sandbox.mynagad.com:10080/remote-payment-gateway-1.0/api/dfs/check-out/initialize/";
        //Example:  http://sandbox.mynagad.com:10080/remote-payment-gateway-1.0/api/dfs/check-out/initialize/" + this.merchantId + "/" + orderId;
        public static string CheckOutAPI = "http://sandbox.mynagad.com:10080/remote-payment-gateway-1.0/api/dfs/check-out/complete/";
        //Example "http://sandbox.mynagad.com:10080/remote-payment-gateway-1.0/api/dfs/check-out/complete/" + sensitiveDataInitializeResponse.getPaymentReferenceId();
        public static string VerifyAPI = "http://sandbox.mynagad.com:10080/remote-payment-gateway-1.0//api/dfs/verify/payment/";
        public string marchentPrivateKey = "MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQCJakyLqojWTDAVUdNJLvuXhROV+LXymqnukBrmiWwTYnJYm9r5cKHj1hYQRhU5eiy6NmFVJqJtwpxyyDSCWSoSmIQMoO2KjYyB5cDajRF45v1GmSeyiIn0hl55qM8ohJGjXQVPfXiqEB5c5REJ8Toy83gzGE3ApmLipoegnwMkewsTNDbe5xZdxN1qfKiRiCL720FtQfIwPDp9ZqbG2OQbdyZUB8I08irKJ0x/psM4SjXasglHBK5G1DX7BmwcB/PRbC0cHYy3pXDmLI8pZl1NehLzbav0Y4fP4MdnpQnfzZJdpaGVE0oI15lq+KZ0tbllNcS+/4MSwW+afvOw9bazAgMBAAECggEAIkenUsw3GKam9BqWh9I1p0Xmbeo+kYftznqai1pK4McVWW9//+wOJsU4edTR5KXK1KVOQKzDpnf/CU9SchYGPd9YScI3n/HR1HHZW2wHqM6O7na0hYA0UhDXLqhjDWuM3WEOOxdE67/bozbtujo4V4+PM8fjVaTsVDhQ60vfv9CnJJ7dLnhqcoovidOwZTHwG+pQtAwbX0ICgKSrc0elv8ZtfwlEvgIrtSiLAO1/CAf+uReUXyBCZhS4Xl7LroKZGiZ80/JE5mc67V/yImVKHBe0aZwgDHgtHh63/50/cAyuUfKyreAH0VLEwy54UCGramPQqYlIReMEbi6U4GC5AQKBgQDfDnHCH1rBvBWfkxPivl/yNKmENBkVikGWBwHNA3wVQ+xZ1Oqmjw3zuHY0xOH0GtK8l3Jy5dRL4DYlwB1qgd/Cxh0mmOv7/C3SviRk7W6FKqdpJLyaE/bqI9AmRCZBpX2PMje6Mm8QHp6+1QpPnN/SenOvoQg/WWYM1DNXUJsfMwKBgQCdtddE7A5IBvgZX2o9vTLZY/3KVuHgJm9dQNbfvtXw+IQfwssPqjrvoU6hPBWHbCZl6FCl2tRh/QfYR/N7H2PvRFfbbeWHw9+xwFP1pdgMug4cTAt4rkRJRLjEnZCNvSMVHrri+fAgpv296nOhwmY/qw5Smi9rMkRY6BoNCiEKgQKBgAaRnFQFLF0MNu7OHAXPaW/ukRdtmVeDDM9oQWtSMPNHXsx+crKY/+YvhnujWKwhphcbtqkfj5L0dWPDNpqOXJKV1wHt+vUexhKwus2mGF0flnKIPG2lLN5UU6rs0tuYDgyLhAyds5ub6zzfdUBG9Gh0ZrfDXETRUyoJjcGChC71AoGAfmSciL0SWQFU1qjUcXRvCzCK1h25WrYS7E6pppm/xia1ZOrtaLmKEEBbzvZjXqv7PhLoh3OQYJO0NM69QMCQi9JfAxnZKWx+m2tDHozyUIjQBDehve8UBRBRcCnDDwU015lQN9YNb23Fz+3VDB/LaF1D1kmBlUys3//r2OV0Q4ECgYBnpo6ZFmrHvV9IMIGjP7XIlVa1uiMCt41FVyINB9SJnamGGauW/pyENvEVh+ueuthSg37e/l0Xu0nm/XGqyKCqkAfBbL2Uj/j5FyDFrpF27PkANDo99CdqL5A4NQzZ69QRlCQ4wnNCq6GsYy2WEJyU2D+K8EBSQcwLsrI7QL7fvQ=="; //Get just the base64 content.
        public string marchentPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAiWpMi6qI1kwwFVHTSS77l4UTlfi18pqp7pAa5olsE2JyWJva+XCh49YWEEYVOXosujZhVSaibcKccsg0glkqEpiEDKDtio2MgeXA2o0ReOb9RpknsoiJ9IZeeajPKISRo10FT314qhAeXOURCfE6MvN4MxhNwKZi4qaHoJ8DJHsLEzQ23ucWXcTdanyokYgi+9tBbUHyMDw6fWamxtjkG3cmVAfCNPIqyidMf6bDOEo12rIJRwSuRtQ1+wZsHAfz0WwtHB2Mt6Vw5iyPKWZdTXoS822r9GOHz+DHZ6UJ382SXaWhlRNKCNeZavimdLW5ZTXEvv+DEsFvmn7zsPW2swIDAQAB";
        public string nagadPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAjBH1pFNSSRKPuMcNxmU5jZ1x8K9LPFM4XSu11m7uCfLUSE4SEjL30w3ockFvwAcuJffCUwtSpbjr34cSTD7EFG1Jqk9Gg0fQCKvPaU54jjMJoP2toR9fGmQV7y9fz31UVxSk97AqWZZLJBT2lmv76AgpVV0k0xtb/0VIv8pd/j6TIz9SFfsTQOugHkhyRzzhvZisiKzOAAWNX8RMpG+iqQi4p9W9VrmmiCfFDmLFnMrwhncnMsvlXB8QSJCq2irrx3HG0SJJCbS5+atz+E1iqO8QaPJ05snxv82Mf4NlZ4gZK0Pq/VvJ20lSkR+0nk+s/v3BgIyle78wjZP1vWLU4wIDAQAB";

        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpNagadClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWalletService _walletService;
        public PaymentController(IHttpContextAccessor httpContextAccessor, IWalletService walletService)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://tokenized.sandbox.bka.sh/v1.2.0-beta/tokenized/checkout/");
            _httpContextAccessor = httpContextAccessor;
            _httpNagadClient = new HttpClient();
            _httpNagadClient.BaseAddress = new Uri("http://sandbox.mynagad.com:10080/remote-payment-gateway-1.0/api/dfs/check-out/");
            _walletService = walletService;
        }

        [HttpPost("bkash/create-payment")]
        //[Authorize]
        public async Task<IActionResult> CreateBkashPayment(PaymentRequest payment)
        {
            string token = await GetGrantToken();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "" + token + "");
            _httpClient.DefaultRequestHeaders.Add("x-app-key", "4f6o0cjiki2rfm34kfdadl1eqq");

            string jsonBody = "{\"mode\": \"0011\", \"payerReference\": \"01\",\"callbackURL\": \"https://api.edex365.com/payment/bkash/callback/\",\"merchantAssociationInfo\": \"MI05MID54RF09123456One\",\"amount\": \"" + payment.Amount + "\", \"currency\": \"BDT\",\"intent\": \"sale\",  \"merchantInvoiceNumber\": \"" + payment.UserId + "_" + Guid.NewGuid() + "\"}";



            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("create", content);
            var responseStr = await response.Content.ReadAsStringAsync();


            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = JObject.Parse(responseStr);
                var resultDetails = new PaymentResponse
                {
                    StatusCode = (int)result["statusCode"],
                    StatusMessage = (string)result["statusMessage"],
                    PaymentID = (string)result["paymentID"],
                    BkashURL = (string)result["bkashURL"],
                    CallbackURL = (string)result["callbackURL"],
                    SuccessCallbackURL = (string)result["successCallbackURL"],
                    FailureCallbackURL = (string)result["failureCallbackURL"],
                    //CancelCallbackURL = (string)result["cancelCallbackURL"],
                    Amount = (string)result["amount"],
                    Currency = (string)result["currency"],
                    PaymentCreateTime = (string)result["paymentCreateTime"],
                    MerchantInvoiceNumber = (string)result["merchantInvoiceNumber"]
                };
                //await _walletService.InsertWalletAsync(resultDetails, payment.UserId);
                var serializedResult = JsonConvert.SerializeObject(resultDetails);

                //var faCode = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);


                _httpContextAccessor.HttpContext.Session.SetString("PaymentResponse", serializedResult);
                await _walletService.InsertWalletAsync(resultDetails, payment.UserId);

                return Ok(resultDetails);
            }
            else
            {
                var resultDetails = new
                {
                    statusCode = 0,
                    statusMessage = "",
                    paymentID = "",
                    bkashURL = "",
                    callbackURL = "",
                    successCallbackURL = "",
                    failureCallbackURL = "",
                    amount = "",
                    currency = "",
                    paymentCreateTime = "",
                    merchantInvoiceNumber = "",
                };
                return Ok(resultDetails);
            }
        }

        private async Task<string> GetGrantToken()
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("username", "sandboxTokenizedUser02");
            _httpClient.DefaultRequestHeaders.Add("password", "sandboxTokenizedUser02@12345");

            string jsonBody = "{\"app_key\": \"4f6o0cjiki2rfm34kfdadl1eqq\", \"app_secret\": \"2is7hdktrekvrbljjh44ll3d9l1dtjo4pasmjvs5vl5qr3fug4b\"}";
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("token/grant", content);
            var responseStr = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<GrantTokenResponse>(responseStr);
                return result?.id_token ?? string.Empty;
            }
            else
            {
                return "";
            }
        }


        [HttpPost("bkash/execute-payment")]
        [Authorize]
        public async Task<IActionResult> ExecuteBkashPayment()
        {
            string token = await GetGrantToken();
            var faCode = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var serializedResult = _httpContextAccessor.HttpContext.Session.GetString(faCode);
            PaymentResponse paymentObj = new PaymentResponse();
            if (serializedResult != null)
            {
                paymentObj = JsonConvert.DeserializeObject<PaymentResponse>(serializedResult);
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "" + token + "");
                _httpClient.DefaultRequestHeaders.Add("x-app-key", "4f6o0cjiki2rfm34kfdadl1eqq");

                string jsonBody = "{\"paymentID\": \"" + paymentObj.PaymentID + "\"}";
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("execute", content);
                var responseStr = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JObject.Parse(responseStr);
                    if ((int)result["statusCode"] == 0)
                    {
                        return new RedirectResult(paymentObj.SuccessCallbackURL);
                    }
                    return new RedirectResult(paymentObj.FailureCallbackURL);
                }
                else
                {
                    return new RedirectResult(paymentObj.FailureCallbackURL);
                }
            }
            return BadRequest("Timeout");
        }
        //https://api.edex365.com/payment/bkash/callback/?paymentID=TR0011JyEDXH21708316182998&status=success
        [HttpGet("bkash/callback")]
        public async Task<IActionResult> ExecuteCallback([FromQuery] string paymentID, [FromQuery] string status)
        {
            if (status == "success")
            {
                return Ok("Successfully Payment");
            }
            else
            {
                return Ok("Payment Failed");
            }
        }


        [HttpGet("Bkash/GrantToken")]
        public async Task<ActionResult<GrantTokenResponse>> GetBkashToken()
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("username", "sandboxTokenizedUser02");
            _httpClient.DefaultRequestHeaders.Add("password", "sandboxTokenizedUser02@12345");

            string jsonBody = "{\"app_key\": \"4f6o0cjiki2rfm34kfdadl1eqq\", \"app_secret\": \"2is7hdktrekvrbljjh44ll3d9l1dtjo4pasmjvs5vl5qr3fug4b\"}";
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("token/grant", content);
            var responseStr = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<GrantTokenResponse>(responseStr);
                return Ok(result);
            }
            else
            {
                return Ok(new GrantTokenResponse());
            }
        }


        [HttpGet("Bkash/RefreshToken")]
        public async Task<ActionResult<RefreshTokenResponse>> GetRefreshToken()
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("username", "sandboxTokenizedUser02");
            _httpClient.DefaultRequestHeaders.Add("password", "sandboxTokenizedUser02@12345");

            string jsonBody = "{\"app_key\": \"4f6o0cjiki2rfm34kfdadl1eqq\", \"app_secret\": \"2is7hdktrekvrbljjh44ll3d9l1dtjo4pasmjvs5vl5qr3fug4b\",\"refresh_token\": \"test_refresh_token\"}";
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("token/grant", content);
            var responseStr = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<RefreshTokenResponse>(responseStr);
                return Ok(result);
            }
            else
            {
                return Ok(new RefreshTokenResponse());
            }
        }
    }
}


//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
//using S_EDex365.API.Models.Payment;
//using System.Net;
//using System.Security.Claims;
//using System.Text;
//using S_EDex365.API.Interfaces;
//using System.Net.Http.Headers;
//using System.Security.Cryptography;

//namespace S_EDex365.API.Controllers.Payment
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PaymentController : ControllerBase
//    {
//        private readonly HttpClient _httpClient;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly IWalletService _walletService;

//        private const string AppKey = "4f6o0cjiki2rfm34kfdadl1eqq";
//        private const string AppSecret = "2is7hdktrekvrbljjh44ll3d9l1dtjo4pasmjvs5vl5qr3fug4b";
//        private const string Username = "sandboxTokenizedUser02";
//        private const string Password = "sandboxTokenizedUser02@12345";

//        public PaymentController(IHttpContextAccessor httpContextAccessor, IWalletService walletService)
//        {
//            _httpClient = new HttpClient
//            {
//                BaseAddress = new Uri("https://tokenized.sandbox.bka.sh/v1.2.0-beta/tokenized/checkout/")
//            };
//            _httpContextAccessor = httpContextAccessor;
//            _walletService = walletService;
//        }

//        [HttpPost("bkash/create-payment")]
//        public async Task<IActionResult> CreateBkashPayment(PaymentRequest payment)
//        {
//            string token = await GetGrantToken();
//            if (string.IsNullOrEmpty(token))
//                return BadRequest("Unable to generate token.");

//            SetBkashHeaders(token);

//            string invoiceNumber = $"{payment.UserId}_{Guid.NewGuid()}";

//            var payload = new
//            {
//                mode = "0011",
//                payerReference = "01",
//                callbackURL = "https://zp9t39vb-44395.asse.devtunnels.ms/payment/bkash/callback/",
//                merchantAssociationInfo = "MI05MID54RF09123456One",
//                amount = payment.Amount,
//                currency = "BDT",
//                intent = "sale",
//                merchantInvoiceNumber = invoiceNumber
//            };

//            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
//            HttpResponseMessage response = await _httpClient.PostAsync("create", content);
//            string responseStr = await response.Content.ReadAsStringAsync();

//            if (response.StatusCode == HttpStatusCode.OK)
//            {
//                var result = JObject.Parse(responseStr);
//                var resultDetails = new PaymentResponse
//                {
//                    StatusCode = (int)result["statusCode"],
//                    StatusMessage = (string)result["statusMessage"],
//                    PaymentID = (string)result["paymentID"],
//                    BkashURL = (string)result["bkashURL"],
//                    CallbackURL = (string)result["callbackURL"],
//                    SuccessCallbackURL = (string)result["successCallbackURL"],
//                    FailureCallbackURL = (string)result["failureCallbackURL"],
//                    Amount = (string)result["amount"],
//                    Currency = (string)result["currency"],
//                    PaymentCreateTime = (string)result["paymentCreateTime"],
//                    MerchantInvoiceNumber = (string)result["merchantInvoiceNumber"]
//                };

//                // Store payment info in session using PaymentID as key
//                _httpContextAccessor.HttpContext.Session.SetString(resultDetails.PaymentID, JsonConvert.SerializeObject(resultDetails));

//                // Optional: save to DB
//                await _walletService.InsertWalletAsync(resultDetails, payment.UserId);

//                return Ok(resultDetails);
//            }

//            return BadRequest("Payment initiation failed.");
//        }


//        [HttpPost("bkash/execute-payment")]
//        public async Task<IActionResult> ExecuteBkashPayment([FromQuery] string paymentID)
//        {
//            if (string.IsNullOrEmpty(paymentID))
//            {
//                return BadRequest("Missing paymentID.");
//            }

//            string token = await GetGrantToken();
//            if (string.IsNullOrEmpty(token))
//            {
//                return BadRequest("Token generation failed.");
//            }

//            // AWS SigV4 Configuration (bKash specific)
//            var region = "ap-southeast-1"; // bKash's region
//            var service = "execute-api";   // Standard for API Gateway
//            var accessKey = AppKey;        // Typically your x-app-key
//            var secretKey = AppSecret;     // Your app secret

//            using var client = new HttpClient
//            {
//                BaseAddress = new Uri("https://tokenized.sandbox.bka.sh/v1.2.0-beta/tokenized/checkout/")
//            };

//            // Generate required timestamp
//            var amzDate = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ");
//            var dateStamp = DateTime.UtcNow.ToString("yyyyMMdd");

//            // Prepare canonical request components
//            var method = "POST";
//            var canonicalUri = $"/tokenized/checkout/execute/{paymentID}";
//            var canonicalQueryString = "";
//            var canonicalHeaders = $"accept:application/json\nhost:tokenized.sandbox.bka.sh\nx-amz-date:{amzDate}\nx-app-key:{AppKey}\n";
//            var signedHeaders = "accept;host;x-amz-date;x-app-key";
//            var payloadHash = ComputeSha256Hash(""); // Empty body for POST execute

//            // Create canonical request
//            var canonicalRequest = $"{method}\n{canonicalUri}\n{canonicalQueryString}\n{canonicalHeaders}\n{signedHeaders}\n{payloadHash}";

//            // Create string to sign
//            var algorithm = "AWS4-HMAC-SHA256";
//            var credentialScope = $"{dateStamp}/{region}/{service}/aws4_request";
//            var stringToSign = $"{algorithm}\n{amzDate}\n{credentialScope}\n{ComputeSha256Hash(canonicalRequest)}";

//            // Calculate signature
//            var signingKey = GetSignatureKey(secretKey, dateStamp, region, service);
//            var signature = ToHexString(HmacSha256(stringToSign, signingKey));

//            // Set headers
//            client.DefaultRequestHeaders.Clear();
//            client.DefaultRequestHeaders.Add("Accept", "application/json");
//            client.DefaultRequestHeaders.Add("x-app-key", AppKey);
//            client.DefaultRequestHeaders.Add("X-Amz-Date", amzDate);
//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
//                algorithm,
//                $"Credential={accessKey}/{credentialScope}, SignedHeaders={signedHeaders}, Signature={signature}"
//            );

//            try
//            {
//                var response = await client.PostAsync($"execute/{paymentID}", null);
//                var responseString = await response.Content.ReadAsStringAsync();

//                if (response.IsSuccessStatusCode)
//                {
//                    return Ok(JsonConvert.DeserializeObject<object>(responseString));
//                }

//                return BadRequest("Execute error: " + responseString);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, "Execute exception: " + ex.Message);
//            }
//        }

//        // Helper methods for AWS SigV4 calculation
//        private static byte[] HmacSha256(string data, byte[] key)
//        {
//            using var hmac = new HMACSHA256(key);
//            return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
//        }

//        private static byte[] GetSignatureKey(string key, string dateStamp, string regionName, string serviceName)
//        {
//            var kSecret = Encoding.UTF8.GetBytes("AWS4" + key);
//            var kDate = HmacSha256(dateStamp, kSecret);
//            var kRegion = HmacSha256(regionName, kDate);
//            var kService = HmacSha256(serviceName, kRegion);
//            return HmacSha256("aws4_request", kService);
//        }

//        private static string ComputeSha256Hash(string text)
//        {
//            using var sha256 = SHA256.Create();
//            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
//            return ToHexString(bytes);
//        }

//        private static string ToHexString(byte[] data)
//        {
//            return BitConverter.ToString(data).Replace("-", "").ToLowerInvariant();
//        }



//        [HttpGet("bkash/callback")]
//        public IActionResult ExecuteCallback([FromQuery] string paymentID, [FromQuery] string status)
//        {
//            return Ok(status == "success" ? "Payment Successful" : "Payment Failed");
//        }

//        [HttpGet("Bkash/GrantToken")]
//        public async Task<ActionResult<GrantTokenResponse>> GetBkashToken()
//        {
//            string token = await GetGrantToken();
//            if (string.IsNullOrEmpty(token))
//                return Ok(new GrantTokenResponse());

//            return Ok(new GrantTokenResponse { id_token = token });
//        }

//        private async Task<string> GetGrantToken()
//        {
//            _httpClient.DefaultRequestHeaders.Clear();
//            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
//            _httpClient.DefaultRequestHeaders.Add("username", Username);
//            _httpClient.DefaultRequestHeaders.Add("password", Password);

//            // Body payload
//            var body = new
//            {
//                app_key = AppKey,
//                app_secret = AppSecret
//            };

//            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

//            try
//            {
//                var response = await _httpClient.PostAsync("token/grant", content);
//                var responseString = await response.Content.ReadAsStringAsync();

//                if (response.IsSuccessStatusCode)
//                {
//                    var result = JsonConvert.DeserializeObject<GrantTokenResponse>(responseString);
//                    return result?.id_token ?? string.Empty;
//                }

//                Console.WriteLine("Token error: " + responseString);
//                return string.Empty;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Token exception: " + ex.Message);
//                return string.Empty;
//            }
//        }


//        private void SetBkashHeaders(string token)
//        {
//            _httpClient.DefaultRequestHeaders.Clear();
//            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
//            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
//            _httpClient.DefaultRequestHeaders.Add("x-app-key", AppKey);
//        }
//    }
//}
