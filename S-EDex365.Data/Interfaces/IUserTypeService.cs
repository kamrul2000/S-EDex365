using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Interfaces
{
    public interface IUserTypeService
    {
        Task<List<UserType>> GetAllUserTypeAsync();
        Task<bool> InsertUserTypeAsync(UserType userType);
        Task<bool> UpdateUserTypeAsync(UserType userType);
        Task<UserType> GetUserTypeByIdAsync(Guid roleId);
        Task<bool> DeleteUserTypeAsync(Guid userTypeId);
    }
}
