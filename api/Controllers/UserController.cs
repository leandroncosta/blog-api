using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Controllers
{

    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {

        private readonly IMongoCollection<User> _usersCollection;

        private readonly IMongoCollection<Post> _postCollections;

        public UserController(MongoDbService mongoDbService)
        {
            _usersCollection = mongoDbService.GetCollection<User>("user");
            _postCollections = mongoDbService.GetCollection<Post>("post");


        }

        [HttpGet]
        public async Task<IActionResult> getUsers()
        {
            var users = await _usersCollection.Find(u => true).ToListAsync();

            return Ok(users);

        }

        [HttpPost]
        public async Task<IActionResult> InsertUser([FromBody] User user)
        {

            if (user == null)
            {
                return BadRequest("Dados do usuário não fornecidos");
            }

            // 1. Insere o usuário e gera o ID
            await _usersCollection.InsertOneAsync(user);






            return Ok(new { message = "User created successfully", userId = user.Id.ToString() });
        }

        [HttpGet("{userId}/posts")]
        public async Task<IActionResult> getPostsByUserId(string userId)
        {

            var user = await _usersCollection.Find(u => u.Id == ObjectId.Parse(userId)).FirstOrDefaultAsync();

            if (user == null) return NotFound("usuário não encontrado");


            if (user.PostsIds != null && user.PostsIds.Any())
            {
                var posts = await _postCollections
                .Find(post => user.PostsIds.Contains(post.Id))
                .ToListAsync();

                return Ok(posts);
            }





            return NoContent();
        }




    
}
}