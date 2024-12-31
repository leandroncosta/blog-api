

using System.Collections.Generic;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace api.Services.PostService
{
    public class PostRepository : IPostRepository
    {
        private readonly IMongoCollection<Post> _postsCollection;
        private readonly IMongoCollection<User> _usersCollection;
        public PostRepository(IMongoDbService mongoDbService)
        {
            _postsCollection = mongoDbService.GetCollection<Post>("post");
            _usersCollection = mongoDbService.GetCollection<User>("user");
        }
        public async Task<List<Post>> GetPosts()
        {
                return await _postsCollection.Find(_ => true).ToListAsync(); ;
        }

        public async Task<Post> GetPostById(string id)
        {
            return await _postsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();  
        }
        public async Task<Post> CreatePost(string userId, Post post)
        {
                post.UserId = userId;
                await _postsCollection.InsertOneAsync(post);
                return post;
        }
        public async Task<List<Post>> GetPostsByUserId(string userId)
        {
                return await _postsCollection.Find(p => p.UserId == userId).ToListAsync();
        }
        public async Task<Post> Put(string id, Post post)
        {
                var postDb = _postsCollection.Find(p => p.Id.Equals(id)).FirstOrDefault();
                
                postDb.Title = post.Title;
                postDb.Content = post.Content;
                await _postsCollection.ReplaceOneAsync(p => p.Id.Equals(id), postDb);
                return postDb;
        }
        public async Task<bool> Delete(string id)
        {
            var postDb = _postsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            var isDeleted = await _postsCollection.DeleteOneAsync(p => p.Id == id);
            if (isDeleted.IsAcknowledged)
                return true;
            else
            {
                return false;
            }

        }

        public async Task DeleteManyByUserIdAsync(string userId)
        {
            var filterUserPosts = Builders<Post>.Filter.Eq(post => post.UserId, userId);
           await _postsCollection.DeleteManyAsync(filterUserPosts);
        }
    }
}

