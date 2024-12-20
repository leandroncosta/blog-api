using System.ComponentModel;
using api.Data;
using api.Models;
using MongoDB.Driver;

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoDbService mongoDbService)
        {

            _collection = mongoDbService.GetCollection<User>("user");

        }

        public async Task AddAsync(User user)
        {
            await _collection.InsertOneAsync(user);
        }

        public async Task DeleteAsync(string userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            await _collection.DeleteOneAsync(filter);

        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await _collection
            .Find(user => user.Id == userId)
            .FirstOrDefaultAsync();
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _collection
            .Find(user => user.UserName == userName)
            .FirstOrDefaultAsync();
        }

        public async Task AddPostIdToUserAsync(string userId, string postId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.AddToSet(u => u.PostsIds, postId);

            await _collection.UpdateOneAsync(filter, update);
        }


        public async Task<User> UpdateAsync(string userId, User updatedUser)
        {
            var filterById = Builders<User>.Filter.Eq(user => user.Id, userId);

            var patchBuilder = Builders<User>.Update;
            var updates = new List<UpdateDefinition<User>>();

            if (!string.IsNullOrEmpty(updatedUser.UserName))
            {
                updates.Add(patchBuilder.Set(p => p.UserName, updatedUser.UserName));
            }

            if (!string.IsNullOrEmpty(updatedUser.Password))
            {
                updates.Add(patchBuilder.Set(p => p.Password, updatedUser.Password));
            }

            var updateDefinition = patchBuilder.Combine(updates);
            return await _collection.FindOneAndUpdateAsync(
              filterById,
              updateDefinition,
               new FindOneAndUpdateOptions<User>
               {
                   ReturnDocument = ReturnDocument.After
               });

        }

        public async Task UpdatePostIdsForUserAsync(string userId, string postId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Pull(u => u.PostsIds, postId);

            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
