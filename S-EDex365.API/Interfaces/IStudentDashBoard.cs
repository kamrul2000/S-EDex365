namespace S_EDex365.API.Interfaces
{
    public interface IStudentDashBoard
    {
        Task<int> GetAllTotalProblemAsync(Guid userId);
        Task<int> GetAllSolutionAsync(Guid userId);
        Task<int> GetAllPendingProblemAsync(Guid userId);
    }
}
