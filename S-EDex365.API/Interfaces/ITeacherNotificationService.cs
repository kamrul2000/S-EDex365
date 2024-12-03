using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface ITeacherNotificationService
    {
        Task<List<ProblemPostAll>> GetAllPostAsync();
        Task<List<ProblemPostAll>> GetAllPostByUserAsync(Guid userId);
        Task<string> GetUserTokenAsync(Guid userId);
        Task<(Guid postId, List<Guid> userIds)> GetStudentProblemAsync(Guid postId);

    }
}
