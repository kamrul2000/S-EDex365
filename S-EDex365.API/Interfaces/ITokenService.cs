
using S_EDex365.API.Models;
using S_EDex365.Model.Model;

namespace S_EDex365.API.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
