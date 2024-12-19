using api.Models;

namespace api.Services.PostService
{
    public interface IPostRepository
    {
        Task<List<Post>> GetPosts();
        Task<Post> GetPostById(string id);
        Task<Post> CreatePost(string userId,Post post);
        Task<List<Post>> GetPostsByUserId(string userId);
        Task<Post> Put(string id, Post post);
        Task<bool> Delete(string id);
    }
}
