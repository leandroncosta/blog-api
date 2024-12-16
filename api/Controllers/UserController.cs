using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace api.Controllers
{

    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {

        private readonly IMongoCollection<User> _usersCollection;

        public UserController(MongoDbService mongoDbService)
        {
            _usersCollection = mongoDbService.GetCollection<User>("User");
        }

        [HttpGet]
        public async Task<IActionResult> getUsers()
        {
            var users = await _usersCollection.Find(_ => true).ToListAsync();
            return Ok(users);

        }

        [HttpPost]
        public async Task<IActionResult> InsertUser([FromBody] User user)
        {
            await _usersCollection.InsertOneAsync(user);

            return CreatedAtAction(nameof(getUsers), new { id = user.Id }, user);
        }

    }
}