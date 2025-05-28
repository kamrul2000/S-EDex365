namespace S_EDex365.API.Models.Payment
{
    public class PaymentResponse
    {
        public Guid UserId { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
        public string PaymentID { get; set; } = string.Empty;
        public string BkashURL { get; set; } = string.Empty;
        public string CallbackURL { get; set; } = string.Empty;
        public string SuccessCallbackURL { get; set; } = string.Empty;
        public string FailureCallbackURL { get; set; } = string.Empty;
        //public string CancelCallbackURL { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string PaymentCreateTime { get; set; } = string.Empty;
        public string MerchantInvoiceNumber { get; set; } = string.Empty;
    }
}
