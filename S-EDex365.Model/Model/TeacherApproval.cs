using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Model.Model
{
    public class TeacherApproval
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Image { get; set; }
        public string AcademicImage { get; set; }
        public string SubjectNames { get; set; }
        public string cv { get; set; }
    }
}
