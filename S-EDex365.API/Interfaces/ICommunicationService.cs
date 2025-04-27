using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface ICommunicationService
    {
        Task<List<CommunicationResponse>> GetAllUserAsync(Guid userId);
        Task<CommunicationResponse> InsertSubjectAsync(CommunicationResponse communicationResponse);
    }
}
