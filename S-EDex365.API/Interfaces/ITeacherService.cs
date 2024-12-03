using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface ITeacherService
    {
        Task<List<ProblemPostAll>> GetAllPostAsync();
        Task<List<ProblemPostAll>> GetAllPostByUserAsync(Guid userId);
        Task<string> UpdateProblemFlagAsync(Guid postId);
        //Task<List<ProblemPostAll>> GetAllPostByUserAsync();

    }
}
