namespace S_EDex365.API.Models.Bkash
{
    public class BkashTokenResponse
    {
        public string id_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}
