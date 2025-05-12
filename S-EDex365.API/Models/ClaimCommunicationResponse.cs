using System.Text.Json.Serialization;

namespace S_EDex365.API.Models
{
    public class ClaimCommunicationResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string VoiceUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Text { get; set; }
        public string Message { get; set; }
        public DateTime GetDate { get; set; }
    }
}
