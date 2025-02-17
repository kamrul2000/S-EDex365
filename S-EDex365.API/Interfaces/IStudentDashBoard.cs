namespace S_EDex365.API.Interfaces
{
    public interface IStudentDashBoard
    {
        Task<string?> GetAllTotalProblemAsync(Guid userId);
        Task<string?> GetAllSolutionAsync(Guid userId);
        Task<string?> GetAllPendingProblemAsync(Guid userId);
    }
}
