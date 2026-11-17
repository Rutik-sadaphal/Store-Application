
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApplicationn.DTOs;
using StoreApplicationn.Repositories;

namespace StoreApplicationn.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        private UserResponseDto MapToResponse(User user)
        {
            if (user == null)
                return null;

            return new UserResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                CreatedAt = user.CreatedAt
            };
        }
        public async Task<List<UserResponseDto>> GetAllUsers()
        {
            var users = await userRepository.GetAllUsers();

            return users.Select(user => MapToResponse(user)).ToList();
        }

   
        public async Task<UserResponseDto> GetUserById(int userId)
        {
            var user = await userRepository.GetById(userId);

            return MapToResponse(user);
        }


        public async Task<UserResponseDto> RegisterUser(UserRegisterDto request)
        {
            if (request == null)
                throw new ArgumentException("Registration details are required.");

            if (await userRepository.EmailExists(request.Email))
                throw new InvalidOperationException("Email address is already registered.");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = passwordHash,
                Phone = request.Phone,
                CreatedAt = DateTime.Now
            };

            userRepository.Add(user);
            await userRepository.SaveChanges();

            return MapToResponse(user);
        }

        
        public async Task<UserResponseDto> UpdateUser(
            int userId, UserUpdateDto request)
        {
            if (request == null)
                throw new ArgumentException("Update details are required.");

            var user = await userRepository.GetById(userId);

            if (user == null)
                return null;

            var existingEmail = await userRepository.GetByEmail(request.Email);

            if (existingEmail != null && existingEmail.UserId != userId)
                throw new InvalidOperationException("Email address is already used by another user.");

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.Phone = request.Phone;

            await userRepository.SaveChanges();

            return MapToResponse(user);
        }

      
        public async Task<bool> DeleteUser(int userId)
        {
            var user = await userRepository.GetById(userId);

            if (user == null)
                return false;

            userRepository.Delete(user);
            await userRepository.SaveChanges();

            return true;
        }

      
        public async Task<UserResponseDto> Login(UserLoginDto request)
        {
            if (request == null)
                throw new ArgumentException("Login details are required.");

            var user = await userRepository.GetByEmail(request.Email);

            if (user == null)
                return null;

            bool isPasswordValid;

            try
            {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(
                    request.Password, user.PasswordHash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                isPasswordValid = false;
            }

            if (!isPasswordValid)
                return null;

            return MapToResponse(user);
        }
    }
}
