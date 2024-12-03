using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Model.Model
{
    public class ProblemsPost
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public string Class {  get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public Guid UserId { get; set; }
    }
}
