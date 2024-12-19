﻿

using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace api.Services.PostService
{
    public class PostService : IPostInterface
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<List<Post>> GetPosts()
        {
            var postsDb = await _postRepository.GetPosts();
            return postsDb;
        }
        public async Task<Post> CreatePost(string userId,Post post)
        {
            post.UserId = userId;
            await _postRepository.CreatePost(userId, post);
            return post;
        }  
        public async Task<List<Post>> GetPostsByUserId(string userId)
        {
            var posts = await _postRepository.GetPostsByUserId(userId);
                return posts;
        }
        public async Task<Post> GetPostById(string id)
        {
            return await _postRepository.GetPostById(id);
        }
        public async Task<Post> Put(string id,Post post)
        {
               return  await _postRepository.Put(id, post);
        }
        public async Task<bool> Delete(string id)
        {
            return await _postRepository.Delete(id);
        }

       
    }
}
