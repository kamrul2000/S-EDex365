namespace S_EDex365.API.Models
{
    public class UserAllInformation
    {
        public string Name { get; set; } = string.Empty;
        public string MobileNo { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public string School { get; set; } = string.Empty;
        public string Image { get; set; }
        //public List<string> sClass { get; set; } = new List<string>();
        //public List<string> Subject { get; set; } = new List<string>();
        public string ClassNames { get; set; }
        public string SubjectNames { get; set; }
    }
}
