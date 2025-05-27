using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface ISolutionPostService
    {
        Task<List<SolutionPostResponse>> InsertSolutionPostAsync(SolutionPostDto solutionPost,Guid postId);
        Task<List<SolutionShowAll>> GetAllSolutionAsync(Guid postId);
    }
}
