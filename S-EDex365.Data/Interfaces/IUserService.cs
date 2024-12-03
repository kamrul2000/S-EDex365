using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Interfaces
{
    public interface IUserService
    {
        Task<string> InsertUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<List<User>> GetAllUserAsync();
        Task<User> GetUserAsync(Guid userId);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<User> GetLoginUserAsync(string userName, string password);
        Task<User> GetUserTypeCheckAsync(string userName);
    }
}
