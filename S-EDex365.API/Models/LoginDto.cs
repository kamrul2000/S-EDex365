using System.ComponentModel.DataAnnotations;

namespace S_EDex365.API.Models
{
    public class LoginDto
    {
        [Required]
        public string MobileNo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DeviceToken { get; set; } = string.Empty;
    }
}
