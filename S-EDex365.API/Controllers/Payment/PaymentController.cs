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

        //[HttpPost("bkash/create-payment")]
        ////[Authorize]
        //public async Task<IActionResult> CreateBkashPayment(PaymentRequest payment)
        //{
        //    string token = await GetGrantToken();
        //    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        //    _httpClient.DefaultRequestHeaders.Add("Authorization", "" + token + "");
        //    _httpClient.DefaultRequestHeaders.Add("x-app-key", "4f6o0cjiki2rfm34kfdadl1eqq");

        //    string jsonBody = "{\"mode\": \"0011\", \"payerReference\": \"01\",\"callbackURL\": \"https://api.edex365.com/payment/bkash/callback/\",\"merchantAssociationInfo\": \"MI05MID54RF09123456One\",\"amount\": \"" + payment.Amount + "\", \"currency\": \"BDT\",\"intent\": \"sale\",  \"merchantInvoiceNumber\": \"" + payment.UserId + "_" + Guid.NewGuid() + "\"}";



        //    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = await _httpClient.PostAsync("create", content);
        //    var responseStr = await response.Content.ReadAsStringAsync();


        //    if (response.StatusCode == HttpStatusCode.OK)
        //    {
        //        var result = JObject.Parse(responseStr);
        //        var resultDetails = new PaymentResponse
        //        {
        //            StatusCode = (int)result["statusCode"],
        //            StatusMessage = (string)result["statusMessage"],
        //            PaymentID = (string)result["paymentID"],
        //            BkashURL = (string)result["bkashURL"],
        //            CallbackURL = (string)result["callbackURL"],
        //            SuccessCallbackURL = (string)result["successCallbackURL"],
        //            FailureCallbackURL = (string)result["failureCallbackURL"],
        //            //CancelCallbackURL = (string)result["cancelCallbackURL"],
        //            Amount = (string)result["amount"],
        //            Currency = (string)result["currency"],
        //            PaymentCreateTime = (string)result["paymentCreateTime"],
        //            MerchantInvoiceNumber = (string)result["merchantInvoiceNumber"]
        //        };
        //        //await _walletService.InsertWalletAsync(resultDetails, payment.UserId);
        //        var serializedResult = JsonConvert.SerializeObject(resultDetails);

        //        //var faCode = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);


        //        _httpContextAccessor.HttpContext.Session.SetString("PaymentResponse", serializedResult);
        //        await _walletService.InsertWalletAsync(resultDetails, payment.UserId);

        //        return Ok(resultDetails);
        //    }
        //    else
        //    {
        //        var resultDetails = new
        //        {
        //            statusCode = 0,
        //            statusMessage = "",
        //            paymentID = "",
        //            bkashURL = "",
        //            callbackURL = "",
        //            successCallbackURL = "",
        //            failureCallbackURL = "",
        //            amount = "",
        //            currency = "",
        //            paymentCreateTime = "",
        //            merchantInvoiceNumber = "",
        //        };
        //        return Ok(resultDetails);
        //    }
        //}



        [HttpPost("bkash/create-payment")]
        public async Task<IActionResult> CreateBkashPayment(PaymentRequest payment)
        {
            string token = await GetGrantToken();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            _httpClient.DefaultRequestHeaders.Add("x-app-key", "4f6o0cjiki2rfm34kfdadl1eqq");

            string invoiceNumber = payment.UserId + "_" + Guid.NewGuid();
            string jsonBody = $@"{{
        ""mode"": ""0011"",
        ""payerReference"": ""01"",
        ""callbackURL"": ""https://api.edex365.com/payment/bkash/callback"",
        ""merchantAssociationInfo"": ""MI05MID54RF09123456One"",
        ""amount"": ""{payment.Amount}"",
        ""currency"": ""BDT"",
        ""intent"": ""sale"",
        ""merchantInvoiceNumber"": ""{invoiceNumber}""
    }}";

            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("create", content);
            string responseStr = await response.Content.ReadAsStringAsync();

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
                    Amount = (string)result["amount"],
                    Currency = (string)result["currency"],
                    PaymentCreateTime = (string)result["paymentCreateTime"],
                    MerchantInvoiceNumber = (string)result["merchantInvoiceNumber"],
                    UserId=payment.UserId
                };

                // Optionally store session or log, but DO NOT insert wallet here
                return Ok(resultDetails);
            }

            return BadRequest("Failed to create bKash payment");
        }

        private Guid ExtractUserIdFromInvoice(string invoice)
        {
            var parts = invoice.Split('_');
            return Guid.Parse(parts[0]);
        }

        [HttpPost("payment/bkash/callback")]
        public async Task<IActionResult> BkashCallback([FromQuery] string paymentID,Guid userId)
        {
            if (string.IsNullOrEmpty(paymentID))
                return BadRequest("Missing paymentID");

            string token = await GetGrantToken();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            _httpClient.DefaultRequestHeaders.Add("x-app-key", "4f6o0cjiki2rfm34kfdadl1eqq");

            // Make sure this matches the actual sandbox/prod endpoint
            var executeUrl = $"https://tokenized.sandbox.bka.sh/v1.2.0-beta/tokenized/checkout/execute";

            // Prepare JSON body with paymentID
            var requestData = new { paymentID = paymentID,userId= userId };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(executeUrl, content);
            string responseStr = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = JObject.Parse(responseStr);
                var resultDetails = new PaymentResponse
                {
                    StatusCode = (int)result["statusCode"],
                    StatusMessage = (string)result["statusMessage"],
                    PaymentID = (string)result["paymentID"],
                    Amount = (string)result["amount"],
                    Currency = (string)result["currency"],
                    PaymentCreateTime = (string)result["paymentExecuteTime"],
                    MerchantInvoiceNumber = (string)result["merchantInvoiceNumber"]
                };
                string statusCode = (string)result["statusCode"];
                string statusMessage = (string)result["statusMessage"];
                if (statusCode != "2056")
                {
                    await _walletService.InsertWalletAsync(resultDetails, userId);
                    return Ok(new { success = true, message = "Payment executed and wallet saved" });
                }
                //Guid userId = ExtractUserIdFromInvoice(resultDetails.MerchantInvoiceNumber);


                return BadRequest(new { success = false, message = "Execute failed"});
            }

            return BadRequest(new { success = false, message = "Execute failed"});
        }



        //[HttpPost("payment/bkash/callback")]
        //public async Task<IActionResult> BkashCallback([FromQuery] string paymentID)
        //{
        //    string token = await GetGrantToken();

        //    _httpClient.DefaultRequestHeaders.Clear();
        //    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        //    _httpClient.DefaultRequestHeaders.Add("Authorization", token);
        //    _httpClient.DefaultRequestHeaders.Add("x-app-key", "4f6o0cjiki2rfm34kfdadl1eqq");

        //    HttpResponseMessage response = await _httpClient.PostAsync($"execute?paymentID={paymentID}", null);
        //    string responseStr = await response.Content.ReadAsStringAsync();

        //    if (response.StatusCode == HttpStatusCode.OK)
        //    {
        //        var result = JObject.Parse(responseStr);
        //        var resultDetails = new PaymentResponse
        //        {
        //            StatusCode = (int)result["statusCode"],
        //            StatusMessage = (string)result["statusMessage"],
        //            PaymentID = (string)result["paymentID"],
        //            Amount = (string)result["amount"],
        //            Currency = (string)result["currency"],
        //            PaymentCreateTime = (string)result["paymentExecuteTime"],
        //            MerchantInvoiceNumber = (string)result["merchantInvoiceNumber"]
        //        };

        //        // Extract UserId from invoice
        //        Guid userId = ExtractUserIdFromInvoice(resultDetails.MerchantInvoiceNumber);

        //        // Insert wallet transaction
        //        await _walletService.InsertWalletAsync(resultDetails, userId);

        //        return Ok(new { success = true, message = "Payment executed and wallet saved" });
        //    }

        //    return BadRequest(new { success = false, message = "Payment execution failed" });
        //}




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

