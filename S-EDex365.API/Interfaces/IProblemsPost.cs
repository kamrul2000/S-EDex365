using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface IProblemsPost
    {
        //Task<ProblemsPostResponse> InsertProblemPostAsync(ProblemsPostDto user);
        Task<List<ProblemPostAll>> GetAllUserAsync(Guid userId);

        Task<(Guid postId, ProblemsPostResponse response)> InsertProblemPostAsync(ProblemsPostDto problemsPost);
    }
}
