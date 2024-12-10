
namespace S_EDex365.API.Models
{
    public class ProblemList
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public string sClass { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public bool Flag { get; set; }
    }
}
