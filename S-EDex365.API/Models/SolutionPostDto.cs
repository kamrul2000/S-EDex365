namespace S_EDex365.API.Models
{
    public class SolutionPostDto
    {
        public Guid TeacherId { get; set; }
        //public IFormFile Photo { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
