namespace S_EDex365.API.Models.Payment
{
    public class RefreshTokenResponse
    {
        public string id_token { get; set; } = string.Empty;
        public string token_type { get; set; } = string.Empty;
        public int expires_in { get; set; }
        public string refresh_token { get; set; } = string.Empty;
    }
}
