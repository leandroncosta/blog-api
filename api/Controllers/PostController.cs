using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Controllers
{

    [ApiController]
    [Route("/api/post")]
    public class PostController : ControllerBase
    {

        private readonly IMongoCollection<Post> _postsCollections;
        private readonly IMongoCollection<User> _usersCollections;


        public  PostController(MongoDbService mongoDbService)
        {
            this._postsCollections = mongoDbService.GetCollection<Post>("post");
            this._usersCollections = mongoDbService.GetCollection<User>("user");

        }


        [HttpGet]
        public async Task<IActionResult> getPosts()
        {
            var posts = await this._postsCollections.Find(_ => true).ToListAsync();
            return Ok(posts);
        }


        [HttpPost]
        public async Task<IActionResult> InsertPost([FromBody] CreatePostDto post)
        {

            if (post == null)
                return BadRequest("sem post para criar");


            var newPost = new Post
            {
                UserId = new ObjectId(post.UserId),
                Title = post.Title,
                Content = post.Content,
                Date = post.Date
            };

            await _postsCollections.InsertOneAsync(newPost);

            var user = await this._usersCollections.Find(u => u.Id == ObjectId.Parse(post.UserId)).FirstOrDefaultAsync(); ;

            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }

            if (user.PostsIds == null)
            {
                user.PostsIds = new List<ObjectId>();
            }

            user.PostsIds.Add(newPost.Id);
            // Atualizar o usuário com o novo PostId
            await _usersCollections.ReplaceOneAsync(u => u.Id == user.Id, user);

            return Ok(new { message = "Post created successfully", postId = newPost.Id.ToString() });
        }

    }
}
