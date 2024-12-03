using S_EDex365.Model.Model;

namespace S_EDex365.API.Models
{
    public class UserDto
    {
        public string Name { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public List<string> Role { get; set; } = new List<string>();
    }
}
