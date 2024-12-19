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

        public async Task CreateUserAsync(User user)
        {   

            await _userRepository.AddAsync(user);
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }
            await _userRepository.DeleteAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
           return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            return await _userRepository.GetByIdAsync(userId);  
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
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            await _userRepository.UpdateAsync(userId, updatedUser);
        }
    }
}

