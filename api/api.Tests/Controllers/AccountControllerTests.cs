using api.Controllers;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace api.Tests
{
    public class AccountControllerTests
    {
        private readonly Mock<IUserService> _mockUserServices;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _mockUserServices = new Mock<IUserService>();
            _mockTokenService = new Mock<ITokenService>();

            _controller = new AccountController(_mockUserServices.Object, _mockTokenService.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var user = new User // cria os dados fakes que serão retornados no mock do repository
            {
                UserName = "testuser",
                Password = BCrypt.Net.BCrypt.HashPassword("testpassword", 4)
            };

            _mockUserServices // mocka o serviço e retorna os dados fakes
                .Setup(service => service.GetUserByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var loginRequest = new User { UserName = "testuser", Password = "testpassword" }; // variável com os dados de acesso

            // Act
            var result = await _controller.Login(loginRequest); 

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidUserCredentials_ReturnsBadRequest()
        {
            //Arrange
            _mockUserServices // mocka o repository e retorna os dados fakes
                .Setup(service => service.GetUserByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var loginRequest = new User { UserName = "invalidUser", Password = "blabla" };

            //Act
            var result = await _controller.Login(loginRequest);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsBadRequest()
        {
            //Arrange
            var user = new User
            {
                UserName = "testedasilva",
                Password = BCrypt.Net.BCrypt.HashPassword("test", 4)
            };

            _mockUserServices 
                .Setup(service => service.GetUserByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var loginRequest = new User { UserName = "testedasilva", Password = "x" };

            //Act
            var result = await _controller.Login(loginRequest);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidHash_ReturnInternalServerError()
        {
            // Arrange
            var user = new User
            {
                UserName = "testuser",
                Password = "invalid-hash-format"
            };

            _mockUserServices 
                .Setup(service => service.GetUserByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var loginRequest = new User { UserName = "testuser", Password = "testpassword" };

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var user = new User
            {
                Id = "1",
                UserName = "testuser",
                Password = BCrypt.Net.BCrypt.HashPassword("testpassword", 4)
            };

            // Mocking the user repository
            _mockUserServices
                .Setup(service => service.GetUserByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            // Mocking the token service to return a fake token
            _mockTokenService
                .Setup(service => service.GenerateToken(It.IsAny<string>(), null))
                .Returns("mockedToken");

            var loginRequest = new User { UserName = "testuser", Password = "testpassword" };

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as TokenResponse;

            Assert.Equal("mockedToken", response.Token);
        }
    }
}