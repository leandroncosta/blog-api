﻿

using System.Diagnostics.Eventing.Reader;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace api.Services.PostService
{
    public class PostService : IPostInterface
    {
        private readonly IUserService _userService;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostService(IPostRepository postRepository, IUserService userService, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userService= userService;
            _userRepository= userRepository;
        }
        public async Task<List<Post>> GetPosts()
        {
            var postsDb = await _postRepository.GetPosts();
            return postsDb;
        }
        public async Task<Post> CreatePost(string userId,Post post)
        {
            post.UserId = userId;
            var createdPost = await _postRepository.CreatePost(userId, post);

          ;

            await _userRepository.AddPostIdToUserAsync(userId, createdPost.Id);

            return createdPost;
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
            var post = await _postRepository.GetPostById(id);
            var userId = post?.UserId;

            await _userRepository.UpdatePostIdsForUserAsync(userId, id);

            return await _postRepository.Delete(id);
        }

      


    }
}

