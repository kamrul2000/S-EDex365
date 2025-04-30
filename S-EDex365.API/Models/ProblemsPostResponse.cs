namespace S_EDex365.API.Models
{
    public class ProblemsPostResponse
    { 
        public List<string> Subject { get; set; } = new List<string>();
        public string Topic { get; set; }
        public List<string> sClass { get; set; } = new List<string>();
        public string Description { get; set; }
        public string Photo { get; set; }
        public Guid UserId { get; set; }
    }
}
