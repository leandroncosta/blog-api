

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
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
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


            var user = await _userRepository.GetByIdAsync(post.UserId);

            if (user.PostsIds == null)
            {
                user.PostsIds = new List<string>();
            }
          
                user.PostsIds.Add(createdPost.Id);
            

            Console.WriteLine(string.Join(", ", user.PostsIds));
            await _userRepository.UpdateAsync(userId, user);

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
            return await _postRepository.Delete(id);
        }

       
    }
}

