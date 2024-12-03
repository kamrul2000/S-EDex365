using S_EDex365.Model.Model;

namespace S_EDex365.API.Models
{
    public class TeacherApprovalResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
