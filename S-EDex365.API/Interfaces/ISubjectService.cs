using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface ISubjectService
    {
        Task<SubjectResponse> InsertSubjectAsync(SubjectResponse subjectResponse);
        Task<List<SubjectResponse>> GetAllSubjectAsync();
    }
}
