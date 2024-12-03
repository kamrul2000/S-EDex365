namespace S_EDex365.API.Models
{
    public class SubjectDtoUpdate
    {
        public Guid userId { get; set; }
        public List<string> Subject { get; set; } = new List<string>();
    }
}
