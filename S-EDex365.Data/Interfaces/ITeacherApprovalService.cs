using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Interfaces
{
    public interface ITeacherApprovalService
    {
        Task<List<TeacherApproval>> GetAllTeacherApprovalListAsync();
        Task<bool> UpdateTeacherApprovalAsync(TeacherApproval teacherApproval);
        Task<TeacherApproval> GetTeacherApprovaByIdAsync(Guid userId);
    }
}
