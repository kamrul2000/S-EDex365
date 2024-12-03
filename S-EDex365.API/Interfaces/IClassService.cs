using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface IClassService
    {
        Task<ClassResponse> InsertClassAsync(ClassResponse classResponse);
        Task<List<ClassResponse>> GetAllClassAsync();
    }
}
