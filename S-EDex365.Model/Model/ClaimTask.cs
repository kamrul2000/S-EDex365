using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Model.Model
{
    public class ClaimTask
    {
        public Guid Id { get; set; }
        public string StudentName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TaskImage { get; set; }
        public string TeacherName { get; set; }
        public string SolutionImage { get; set; }
        public DateTime GetDateby { get; set; }
    }
}
