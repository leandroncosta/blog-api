using api.Models;

namespace api.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(CreateUserDto user); 
        Task<IEnumerable<User>> GetAllUsersAsync(); 
        Task<User> GetUserByIdAsync(string userId); 
        Task<User> GetUserByUserNameAsync(string userName); 
        Task<User> UpdateUserAsync(string userId, UpdateUserDto updateUserDto); 
        Task DeleteUserAsync(string userId); 
    }
}