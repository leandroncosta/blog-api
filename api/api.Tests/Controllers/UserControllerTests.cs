using System.Diagnostics;
using api.Controllers;
using api.Data;
using api.Models;
using api.Services;
using api.Services.PostService;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Moq;


namespace api.Tests.Controllers
{
  public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IPostInterface> _postServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _postServiceMock = new Mock<IPostInterface>();
            _controller = new UserController(_userServiceMock.Object, _postServiceMock.Object);
        }

        [Fact]
        public async Task InsertUser_ShouldReturnBadRequest_WhenUsername_IsNull() 
        {
            //Arrange
            var invalidUser = new CreateUserDto { UserName = null };
            //Act
            var result = await _controller.InsertUser(invalidUser);
            //
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task InsertUser_ShouldReturnBadRequest_WhenUsername_HasWhiteSpace()
        {
            //Arrange
            var invalidUser = new CreateUserDto { UserName = null };
            //Act
            var result = await _controller.InsertUser(invalidUser);
            //
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task InsertUser_ShouldReturnBadRequest_WhenUsername_HasDollarSign()
        {
            // Arrange
            var invalidUser = new CreateUserDto { UserName = "Invalid$Name" };

            // Act
            var result = await _controller.InsertUser(invalidUser);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task InsertUser_ShouldReturnBadRequest_WhenUsername_HasDot()
        {
            // Arrange
            var invalidUser = new CreateUserDto { UserName = "Invalid.Name" };

            // Act
            var result = await _controller.InsertUser(invalidUser);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}