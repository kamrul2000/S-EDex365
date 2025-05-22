using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Interfaces
{
    public interface ISubjectService
    {
        Task<List<Subject>> GetAllSubjectAsync();
        Task<bool> InsertSubjectAsync(Subject subject);
        Task<Subject> GetSubjectByIdAsync(Guid subjectId);
        Task<bool> UpdateSubjectsAsync(Subject subject);
        Task<bool> DeleteSubjectAsync(Guid subjectId);
    }
}
