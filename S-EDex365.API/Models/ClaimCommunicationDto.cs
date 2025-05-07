namespace S_EDex365.API.Models
{
    public class ClaimCommunicationDto
    {
        public IFormFile VoiceUrl { get; set; }
        public IFormFile ImageUrl { get; set; }
        public string Text { get; set; }
    }
}
