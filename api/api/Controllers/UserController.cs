using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(new ResponseDto<List<User>>.Builder()
                            .SetStatus(200)
                            .SetMessage("Usuários encontrados")
                            .SetData(users)
                            .Build());

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertUser([FromBody] CreateUserDto user)
        {
            await _userService.CreateUserAsync(user);
            return Created("", new ResponseDto<User>.Builder()
                .SetStatus(201)
                .SetMessage("Usuário criado com sucesso")
                .SetData(user)
                .Build());
        }

        //[HttpGet("{userId}/posts")]
        //public async Task<IActionResult> getPostsByUserId(string userId)
        //{

        //    var user = await _userService.GetUserByIdAsync(userId);

        //    if (user == null) return NotFound("usuário não encontrado");


        //    if (user.PostsIds != null && user.PostsIds.Any())
        //    {
        //        var posts = await _postCollections
        //        .Find(post => user.PostsIds.Contains(post.Id))
        //        .ToListAsync();

        //        return Ok(new ResponseDto<List<Post>>.Builder()
        //        .SetStatus(200)
        //        .SetMessage("Posts do usuário encontrado")
        //        .SetData(posts)
        //        .Build<List<Post>>()
        //        );
        //    }
        //    return NoContent();
        //    }
        //}


        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto Data, string userId)
        {
            await _userService.UpdateUserAsync(
                userId,
                new User { UserName = Data.Username, Password = Data.Password });

            return Ok(new ResponseDto<User>.Builder()
                .SetStatus(200)
                .SetMessage("Usuário atualizado com sucesso")
                .Build()
                );
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            await _userService.DeleteUserAsync(userId);

            return Ok(new ResponseDto<object>.Builder()
                .SetStatus(200)
                .SetMessage("Usuário deletado com sucesso")
                .Build()
                );
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            return Ok(new ResponseDto<User>.Builder()
               .SetStatus(200)
               .SetMessage("Usuário encontrado com sucesso")
               .SetData(user)
               .Build()
               );
        }

    }
}