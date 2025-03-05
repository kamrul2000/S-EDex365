namespace S_EDex365.API.Models.Payment
{
    public class PaymentRequest
    {
        public Guid UserId { get; set; }
        public double Amount { get; set; }
    }
}
