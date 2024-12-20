using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("/api/login")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;


        public AccountController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User userlogin)
        {
            try
            {
                // Busca o usuario no banco de dados pelo username
                var user = await _userService.GetUserByUserNameAsync(userlogin.UserName);

                // Se o usuario não for encontrado
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid credentials: User not found" });
                }
                if (!user.Password.StartsWith("$2b$") && !user.Password.StartsWith("$2a$")) //verifica se a senha do usuario no banco esta com hash
                {
                    return StatusCode(500, new { message = "Internal server error", error = "Invalid hash format" });
                }
                if (!BCrypt.Net.BCrypt.Verify(userlogin.Password, user.Password)) // compara hash com senha fornecida
                {
                    return BadRequest(new { message = "Invalid credentials: Incorrect password" });
                }

                var token = _tokenService.GenerateToken(user.Id);//gera token e armazena o id do usuario 
                return Ok(new TokenResponse { Token = token }); //retorna o token
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}