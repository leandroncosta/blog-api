using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace api.Controllers
{
    [Authorize]
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


            return Ok(new ResponseDto<List<User>>.Builder()
                 .SetStatus(200)
                 .SetMessage("Usuários encontrados")
                 .SetData(users)
                 .Build<List<User>>());

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertUser([FromBody] User user)
        {

            if (user == null)
            {
                return BadRequest("Dados do usuário não fornecidos");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;
            await _usersCollection.InsertOneAsync(user);

            return Created("", new ResponseDto<User>.Builder()
                .SetStatus(201)
                .SetMessage("Usuário criado com sucesso")
                .SetData(user)
                .Build<User>());
        }

        [HttpGet("{userId}/posts")]
        public async Task<IActionResult> getPostsByUserId(string userId)
        {

            var user = await _usersCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();

            if (user == null) return NotFound("usuário não encontrado");


            if (user.PostsIds != null && user.PostsIds.Any())
            {
                var posts = await _postCollections
                .Find(post => user.PostsIds.Contains(post.Id))
                .ToListAsync();

                return Ok(new ResponseDto<List<Post>>.Builder()
                .SetStatus(200)
                .SetMessage("Posts do usuário encontrado")
                .SetData(posts)
                .Build<List<Post>>()
                );
            }





            return NoContent();
        }


        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto Data, string userId)
        {
            var filterBuilder = Builders<User>.Filter;
            var filterById = filterBuilder.Eq(user => user.Id, userId);


            var user = await _usersCollection.Find(filterById).FirstOrDefaultAsync();
            if (user == null) return NotFound("Usuário não encontrado");

            var patchBuilder = Builders<User>.Update;
            var updates = new List<UpdateDefinition<User>>();

            if (!string.IsNullOrEmpty(Data.Username))
            {

                updates.Add(patchBuilder.Set(p => p.UserName, Data.Username));
            }

            if (!string.IsNullOrEmpty(Data.Password))
            {

                updates.Add(patchBuilder.Set(p => p.Password, Data.Password));
            }
            if (!updates.Any())
                return NoContent();


            var updateDefinition = patchBuilder.Combine(updates);

            var result = await _usersCollection.UpdateOneAsync(
                filterById,
                updateDefinition);


            return Ok(new ResponseDto<User>.Builder()
                .SetStatus(200)
                .SetMessage("Usuário atualizado com sucesso")
                .Build<User>()
                );
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {

            var filterBuilder = Builders<User>.Filter;
            var filterById = filterBuilder.Eq(user => user.Id, userId);


            var user = await _usersCollection.Find(filterById).FirstOrDefaultAsync();
            if (user == null) return NotFound("Usuário não encontrado");

            await _usersCollection.DeleteOneAsync(filterById);
            return Ok(new ResponseDto<object>.Builder()
                .SetStatus(200)
                .SetMessage("Usuário deletado com sucesso")
                .Build<object>()
                );
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> getUserById(string userId)
        {
            var filterBuilder = Builders<User>.Filter;
            var filterById = filterBuilder.Eq(user => user.Id, userId);


            var user = await _usersCollection.Find(filterById).FirstOrDefaultAsync();
            if (user == null) return NotFound("Usuário não encontrado");


            return Ok(new ResponseDto<User>.Builder()
               .SetStatus(200)
               .SetMessage("Usuário encontrado com sucesso")
               .SetData(user)
               .Build<User>()
               );
        }

    }
}