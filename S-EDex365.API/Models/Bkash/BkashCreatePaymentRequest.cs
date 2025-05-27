namespace S_EDex365.API.Models.Bkash
{
    public class BkashCreatePaymentRequest
    {
        public string mode { get; set; } = "0011";
        public string payerReference { get; set; }
        public string callbackURL { get; set; }
        public string amount { get; set; }
        public string currency { get; set; } = "BDT";
        public string intent { get; set; } = "sale";
        public string merchantInvoiceNumber { get; set; }
    }
}
