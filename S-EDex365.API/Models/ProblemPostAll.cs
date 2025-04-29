namespace S_EDex365.API.Models
{
    public class ProblemPostAll
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public string sClass { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public DateTime GetDateby { get; set; }

        public List<CommunicationResponse> TeacherChats { get; set; }
        public List<CommunicationResponse> StudentChats { get; set; }
    }
}
