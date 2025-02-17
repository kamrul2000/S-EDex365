using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Models;
using S_EDex365.Model.Model;

namespace S_EDex365.API.Interfaces
{
    public interface IAuthService
    {
            Task<LoginResponse> Login(LoginDto loginDto);
            Task<RefreshTokenModel> RefreshToken(string userId);
            Task<UserResponse> InsertUserAsync(UserDto user);
            Task<UserResponseUpdate> UpdateUserAsync(UserDtoUpdate userDtoUpdate);
        Task<List<UserAllInformation>> GetAllUserInformationAsync(Guid userId);
        Task<bool> UpdatePasswordserAsync(Guid userId,string oldPassWord,string newPassword);
    }
}
