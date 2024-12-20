using api.Models;
using MongoDB.Driver;

namespace api.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string userId);
        Task<User> GetByUserNameAsync(string userName);
        Task AddPostIdToUserAsync(string userId, string postId);
        Task<User> UpdateAsync(string userId, User updatedUser);
        Task DeleteAsync(string userId);
        Task UpdatePostIdsForUserAsync(string userId, string postId);
    }
}