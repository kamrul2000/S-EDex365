namespace S_EDex365.API.Models.Payment
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string TransactionId { get; set; }
        public float Amount { get; set; }
    }
}
