using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Model.Model
{
    public class EnglishClass
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
        public decimal Amount { get; set; }
        public long S_ClassName { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}
