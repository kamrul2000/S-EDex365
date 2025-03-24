using S_EDex365.Model.Model;

namespace S_EDex365.API.Models
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string Image { get; set; }=string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public string OTP { get; set; } = string.Empty;
        public string CV { get; set; } = string.Empty;
        public string AcademicImage { get; set; } = string.Empty;
        public List<string> Subject { get; set; } = new List<string>();
        public List<string> Role { get; set; } = new List<string>();
    }
}
