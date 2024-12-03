namespace S_EDex365.API.Models
{
    public class ProblemsPostDto
    {
        public List<string> Subject { get; set; } = new List<string>();
        public string Topic { get; set; } = string.Empty;
        public List<string> sClass { get; set; } = new List<string>();
        public string Description { get; set; }=string.Empty;
        public IFormFile Photo { get; set; }
        public Guid UserId { get; set; }
    }
}
