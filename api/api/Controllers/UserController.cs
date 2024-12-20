using api.Models;
using api.Services;
using api.Services.PostService;
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
        private readonly IPostInterface _postService;

        public UserController(IUserService userService, IPostInterface postInterface)
        {
            _userService = userService;
            _postService = postInterface;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var usersDtos = users.Select(UserDTO.ConvertToUserDto).ToList();
            return Ok(new ResponseDto<List<UserDTO>>.Builder()
                            .SetStatus(200)
                            .SetMessage("Usuários encontrados")
                            .SetData(usersDtos)
                            .Build());

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertUser([FromBody] CreateUserDto user)
        {
            await _userService.CreateUserAsync(user);
            return Created("", new ResponseDto<CreateUserDto>.Builder()
                .SetStatus(201)
                .SetMessage("Usuário criado com sucesso")
                .Build());
        }

        [HttpGet("{userId}/posts")]
        public async Task<IActionResult> getPostsByUserId(string userId)
        {
            var posts = await _postService.GetPostsByUserId(userId);
            return Ok(new ResponseDto<List<Post>>.Builder()
                .SetStatus(200)
                .SetMessage("Posts encontrado com sucesso.")
                .SetData(posts)
                .Build()
                );

        }


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

            return Ok(new ResponseDto<UserDTO>.Builder()
               .SetStatus(200)
               .SetMessage("Usuário encontrado com sucesso")
               .SetData(UserDTO.ConvertToUserDto(user))
               .Build()
               );
        }

    }
}
