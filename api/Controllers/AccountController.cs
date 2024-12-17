using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Controllers
{
    [ApiController]
    [Route("/api/login")]
    public class AccountController : ControllerBase
    {
        private readonly IMongoCollection<User> _usersCollection;

        public AccountController(MongoDbService mongoDbService)
        {
            _usersCollection = mongoDbService.GetCollection<User>("user");
        }

        [HttpPost]
        public IActionResult Login([FromBody] User userlogin)
        {
            try
            {
                // Busca o usuario no banco de dados pelo username
                var user = _usersCollection.Find(u => u.UserName == userlogin.UserName).FirstOrDefault();

                // Se o usuario não for encontrado
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid credentials: User not found" });
                }

                // Logar o usuario encontrado
                Console.WriteLine($"User found: {user.UserName}");

                // Verifica se a senha fornecida corresponde ao hash armazenado
                if (!BCrypt.Net.BCrypt.Verify(userlogin.Password, user.Password)) // compara hash com senha fornecida
                {
                    return BadRequest(new { message = "Invalid credentials: Incorrect password" });
                }

                // Gere o token com o ID do usuario (obtido do banco)
                var token = GenerateToken(user.Id);  // Passa o ID do usuario que foi encontrado no banco

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        private string GenerateToken(string userId)
        {
            string secretKey = "d33b5e2e-e925-40c3-9991-f84aaab0825c";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "blogapi",
                audience: "blogapi",
                claims: new[] { new Claim("userId", userId) },
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credential
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}