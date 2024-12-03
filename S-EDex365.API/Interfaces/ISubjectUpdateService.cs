using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface ISubjectUpdateService
    {
        Task<SubjectResponseUpdate> UpdateSubjectAsync(SubjectDtoUpdate subjectDtoUpdate);
        Task<List<SubjectResponseUpdate>> GetAllSubjectResponseUpdateAsync(Guid userId);
    }
}
