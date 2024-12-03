using S_EDex365.API.Models;

namespace S_EDex365.API.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleResponse>> GetRolesAsync();
    }
}
