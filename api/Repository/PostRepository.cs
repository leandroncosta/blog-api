

using System.Collections.Generic;
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
        public PostRepository(MongoDbService mongoDbService)
        {
            _postsCollection = mongoDbService.GetCollection<Post>("Post");
            _usersCollection = mongoDbService.GetCollection<User>("User");
        }
        public async Task<List<Post>> GetPosts()
        {
            List<Post> postsDb;
            try
            {
                return postsDb = await _postsCollection.Find(_ => true).ToListAsync(); ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Post> GetPostById(string id)
        {
            try
            {
                var post = await _postsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
                return post;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }


        public async Task<Post> CreatePost(string userId, Post post)
        {
            post.UserId = userId;
            try
            {
                await _postsCollection.InsertOneAsync(post);
                return post;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Post>> GetPostsByUserId(string userId)
        {
            try
            {
                return await _postsCollection.Find(p => p.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Post> Put(string id, Post post)
        {
            try
            {
                var postDb = _postsCollection.Find(p => p.Id.Equals(id)).FirstOrDefault();
                if (postDb == null)
                {
                    return postDb = new Post();
                }
                postDb.Title = post.Title;
                postDb.Content = post.Content;
                await _postsCollection.ReplaceOneAsync(p => p.Id.Equals(id), postDb);
                return postDb;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
    }
}

