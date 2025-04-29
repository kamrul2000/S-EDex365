using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface IPostTypeService
    {
        Task<List<PostType>> GetAllPostTypeAsync();
    }
}
