using System.ComponentModel.DataAnnotations;
using api.Exceptions;
using api.Models;
using api.Repositories;

namespace api.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateUserAsync(CreateUserDto user)
        {
            var newUser = new User { UserName = user.UserName, Password = user.Password};
            ValidateUser(newUser);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            newUser.Password = hashedPassword;
            await _userRepository.AddAsync(newUser);
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await EnsureUserExistsAsync(userId);
            await _userRepository.DeleteAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
           return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await EnsureUserExistsAsync(userId);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null)
            {
                throw new NotFoundException($"User with userName {userName} not found.");
            }
            return await _userRepository.GetByUserNameAsync(userName);
        }

        public async Task UpdateUserAsync(string userId, User updatedUser)
        {
            var user = await EnsureUserExistsAsync(userId);
            user.UserName = updatedUser.UserName ?? user.UserName;
         
            await _userRepository.UpdateAsync(userId, user);
        }

        private async Task<User> EnsureUserExistsAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            return user;
        }

        private void ValidateUser(User user)
        {
            if (user == null)
            {
                throw new ValidationException("User cannot be null.");
            }

            if (string.IsNullOrEmpty(user.UserName))
            {
                throw new ValidationException("UserName is required.");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                throw new ValidationException("Password is required.");
            }

        }
    }
}

