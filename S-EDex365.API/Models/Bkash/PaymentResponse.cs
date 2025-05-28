namespace S_EDex365.API.Models.Bkash
{
    public class PaymentResponse
    {
        public Guid UserId { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string PaymentID { get; set; }
        public string BkashURL { get; set; }
        public string CallbackURL { get; set; }
        public string SuccessCallbackURL { get; set; }
        public string FailureCallbackURL { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentCreateTime { get; set; }
        public string MerchantInvoiceNumber { get; set; }
        
    }
}
