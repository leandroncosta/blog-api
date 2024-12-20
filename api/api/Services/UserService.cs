using System.ComponentModel.DataAnnotations;
using api.Exceptions;
using api.Models;
using api.Repositories;
using api.Services.PostService;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;

namespace api.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public UserService(IUserRepository userRepository, IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        public async Task<User> CreateUserAsync(CreateUserDto user)
        {
            await EnsureUserDoesNotExistAsync(user.UserName);
            var newUser = new User {
                UserName = user.UserName,
                Password = user.Password,
                PostsIds = new List<string>()
            };
            ValidateUser(newUser);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            newUser.Password = hashedPassword;
     ;
            await _userRepository.AddAsync(newUser);
            return newUser;
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await EnsureUserExistsAsync(userId);
            if(user.PostsIds != null || user.PostsIds?.Count != 0) await EnsureThatUserPostsAreDeletedAsync(userId);
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
            return user; ;
        }

        public async Task<User> UpdateUserAsync(string userId, UpdateUserDto updateUserDto)
        {
            await EnsureUserExistsAsync(userId);
            var user = new User()
            {
                     UserName = updateUserDto.Username,
                     Password = updateUserDto.Password
            };
            return await _userRepository.UpdateAsync(userId, user);
        }

        private async Task EnsureThatUserPostsAreDeletedAsync(string userId) 
        {
            await _postRepository.DeleteManyByUserIdAsync(userId);
        }

        private async Task<User> EnsureUserExistsAsync(string userId)
        {
         
            bool isValid = ObjectId.TryParse(userId, out _);

            if(!isValid) throw new NotFoundException($"User with ID {userId} not found.");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            return user;
        }

        private async Task EnsureUserDoesNotExistAsync(string userName)
        {
            var existingUser = await _userRepository.GetByUserNameAsync(userName);
            if (existingUser != null)
            {
                throw new ValidationException($"User with username '{userName}' already exists.");
            }
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

