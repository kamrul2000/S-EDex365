using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface IClaimCommunicationService
    {
        Task<List<ClaimCommunicationResponse>> GetAllClaimCommunicationAsync();
        Task InsertClaimCommunicationAsync(ClaimCommunicationDto claimCommunicationDto, Guid userId, Guid problempostId);
    }
}
