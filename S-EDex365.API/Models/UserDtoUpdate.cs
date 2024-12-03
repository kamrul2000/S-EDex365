namespace S_EDex365.API.Models
{
    public class UserDtoUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public string School { get; set; } = string.Empty;
        public IFormFile Image { get; set; }
        public List<string> sClass { get; set; } = new List<string>();
        public List<string> Subject { get; set; } = new List<string>();
    }
}
