
using System.Collections.Generic;
using System.Threading.Tasks;
using StoreApplicationn.DTOs;

namespace StoreApplicationn.Services
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllUsers();

        Task<UserResponseDto> GetUserById(int userId);

        Task<UserResponseDto> RegisterUser(UserRegisterDto request);

        Task<UserResponseDto> UpdateUser(int userId, UserUpdateDto request);

        Task<bool> DeleteUser(int userId);

        Task<UserResponseDto> Login(UserLoginDto request);
    }
}
