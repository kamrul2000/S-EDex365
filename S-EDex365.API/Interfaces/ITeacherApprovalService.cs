using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface ITeacherApprovalService
    {
        Task<UserResponse> UpdateTeacherApprovalAsync(TeacherApprovalDto teacherApprovalDto);
        Task<List<TeacherApprovalResponse>> GetAllTeacherApprovalListAsync();
    }
}
