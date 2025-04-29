using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface ICommunicationService
    {
        Task InsertChatAsync(CommunicationDto communicationDto, Guid userId, Guid problempostId);
    }
}
