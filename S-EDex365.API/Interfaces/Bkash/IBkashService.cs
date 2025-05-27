using S_EDex365.API.Models.Bkash;

namespace S_EDex365.API.Interfaces.Bkash
{
    public interface IBkashService
    {
        Task<string> GetTokenAsync();
        Task<BkashCreatePaymentResponse> CreatePaymentAsync(string token, BkashCreatePaymentRequest model);
        Task<BkashExecutePaymentResponse> ExecutePaymentAsync(string token, string paymentId);

    }
}
