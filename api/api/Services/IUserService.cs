using api.Models;

namespace api.Services
{
    public interface IUserService
    {
        Task CreateUserAsync(User user); 
        Task<IEnumerable<User>> GetAllUsersAsync(); 
        Task<User> GetUserByIdAsync(string userId); 
        Task<User> GetUserByUserNameAsync(string userName); 
        Task UpdateUserAsync(string userId, User updatedUser); 
        Task DeleteUserAsync(string userId); 
    }
}
