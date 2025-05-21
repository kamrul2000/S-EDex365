namespace S_EDex365.API.Models.Payment
{
    public class GrantTokenResponse
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; } = string.Empty;
        public string id_token { get; set; } = string.Empty;
        public string token_type { get; set; } = string.Empty;
        public int expires_in { get; set; }
        public string refresh_token { get; set; } = string.Empty;
        //public string id_token { get; set; }
        //public string refresh_token { get; set; }
        //public string token_type { get; set; }
        //public string expires_in { get; set; }
    }
}
