using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface IFirebaseNotificationService
    {
        Task<string> SendNotificationAsync(string title, string body, string token,Guid postId, string photo, string subjectName, string description);
        Task<string> GetAllInfo(Guid postId);
    }
}
