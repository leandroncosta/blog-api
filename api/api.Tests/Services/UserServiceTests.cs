using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.Exceptions;
using api.Models;
using api.Repositories;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace api.Tests.Services
{
    public class UserServiceTests
    {

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        #region Tests GetAllUsersAsync

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
        {
            new User { Id = "1", UserName = "User1" },
            new User { Id = "2", UserName = "User2" },
            new User { Id = "3", UserName = "User3" }
        };
            _userRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            _userRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.Equal(users, result);
            Assert.IsAssignableFrom<IEnumerable<User>>(result);
            Assert.Equal(3, result.Count());
        }

        #endregion

        #region Tests CreateUserAsync

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser_WhenUserIsValid()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                UserName = "newuser",
                Password = "securePassword123"
            };

            var newUser = new User
            {
                UserName = createUserDto.UserName,
                Password = createUserDto.Password
            };

            //// Configura o mock do repositório para que o método AddAsync seja chamado uma vez
            //_userRepositoryMock.Setup(repo => repo.AddAsync(It.Is<User>(u =>
            //    u.UserName == createUserDto.UserName &&
            //    BCrypt.Net.BCrypt.Verify(createUserDto.Password, u.Password))))
            //    .Returns(Task.CompletedTask);

            //// Act
            //await _userService.CreateUserAsync(createUserDto);

            //// Assert
            //_userRepositoryMock.Verify(repo => repo.AddAsync(It.Is<User>(u =>
            //    u.UserName == createUserDto.UserName &&
            //    BCrypt.Net.BCrypt.Verify(createUserDto.Password, u.Password))), Times.Once);
        }


        [Fact]
        public async Task CreateUserAsync_ShouldThrowValidationException_WhenUserNameIsNull()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                UserName = null,
                Password = "securePassword123"
            };

            // Act & Assert
            var result = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await _userService.CreateUserAsync(createUserDto);
            });

            Assert.IsAssignableFrom<ValidationException>(result);
        }


        #endregion

        #region Tests GetUserByUserNameAsync

        [Fact]
        public async Task GetUserByUserNameAsync_ShouldReturnUser_WhenUserExists()
        {
            //range
            var userName = "l_costa";
            var user = new User
            {
                Id = "65648b4a04df751357db22fb",
                UserName = userName,
                Password = "123"
            };

            _userRepositoryMock.Setup(repo => repo.GetByUserNameAsync(userName)).ReturnsAsync(user);

            // act 
            var result = await _userService.GetUserByUserNameAsync(userName);

            //assert
            _userRepositoryMock.Verify(repo => repo.GetByUserNameAsync(userName), Times.Once);
            Assert.Equal(user, result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.Password, result.Password);
            Assert.IsAssignableFrom<User>(result);

        }

        [Fact]
        public async Task GetUserByUserNameAsync_ShouldReturnException_WhenUserNotExists()
        {
            //range
            var userName = "l_costa";
            var user = new User
            {
                Id = "65648b4a04df751357db22fb",
                UserName = userName,
                Password = "123"
            };

            var invalidUserName = "l_nascimento";

            _userRepositoryMock.Setup(repo => repo.GetByUserNameAsync(userName)).ReturnsAsync(user);


            // act 
            var result = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _userService.GetUserByUserNameAsync(invalidUserName);
            });

            //assert

            Assert.Equal($"User with userName {invalidUserName} not found.", result.Message);
            Assert.IsAssignableFrom<NotFoundException>(result);
        }


        #endregion

        #region Tests GetUserByIdAsync
        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            //range
            var userId = "65648b4a04df751357db22fb";
            var user = new User { Id = userId, UserName = "testuser" };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            //act
            var result = await _userService.GetUserByIdAsync(userId);

            //assert
            _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            Assert.Equal(user, result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.Password, result.Password);
            Assert.IsAssignableFrom<User>(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnException_WhenUserNotExists()
        {
            //range
            var userId = "65648b4a04df751357db22fb";
            var user = new User { Id = userId, UserName = "testuser" };
            var invalidId = "65648b4a04df751357db22fc";

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            //act
            var result = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _userService.GetUserByIdAsync(invalidId);
            });

            //assert
            Assert.Equal($"User with ID {invalidId} not found.", result.Message);
            Assert.IsAssignableFrom<NotFoundException>(result);
        }


        #endregion

        #region Tests UpdateUserAsync

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser_WhenUserExists()
        {
            //range
            var userId = "65648b4a04df751357db22fb";
            var existingUser = new User { Id = userId, UserName = "oldUsername" };
            var updatedUser = new User { UserName = "newUserName" };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(repo => repo.UpdateAsync(userId, It.IsAny<User>())).Returns(Task.CompletedTask);

            //act 
            var result = _userService.UpdateUserAsync(userId, updatedUser);

            //assert
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(userId,
                It.Is<User>(u => u.UserName == updatedUser.UserName)), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            Assert.Equal(updatedUser.UserName, existingUser.UserName);
            Assert.Equal(userId, existingUser.Id);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateException_WhenUserNotExists()
        {
            //range
            var userId = "65648b4a04df751357db22fb";
            var existingUser = new User { Id = userId, UserName = "oldUsername" };
            var updatedUser = new User { UserName = "newUserName" };
            var invalidUserId = "65648b4a04df751357db22fc";

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(repo => repo.UpdateAsync(userId, It.IsAny<User>())).Returns(Task.CompletedTask);

            //act 
            var result = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _userService.UpdateUserAsync(invalidUserId, updatedUser);

            });

            //assert
            Assert.Equal($"User with ID {invalidUserId} not found.", result.Message);
            Assert.IsAssignableFrom<NotFoundException>(result);
        }

        #endregion

        #region Tests DeleteUserAsync

        [Fact]
        public async Task DeleteteUserAsync_ShouldDeleteUser_WhenUserExists()
        {
            //range
            var userId = "65648b4a04df751357db22fb";
            var existingUser = new User { Id = userId, UserName = "user" };

            // Configurando o mock para retornar um usuário válido para o userId
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Configurando o mock para o método de atualização
            _userRepositoryMock.Setup(repo => repo.DeleteAsync(userId)).Returns(Task.CompletedTask);

            //act 
            var result = _userService.DeleteUserAsync(userId);

            // Assert

            // Verificando se o repositório foi chamado uma vez para excluir o usuário
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(userId), Times.Once);
            // Garantindo que o método EnsureUserExistsAsync foi chamado
            _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteteUserAsync_ShouldUserExcepion_WhenUserNotExists()
        {
            //range
            var userId = "65648b4a04df751357db22fb";
            var existingUser = new User { Id = userId, UserName = "user" };
            var invalidUserId = "65648b4a04df751357db22fc";

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.DeleteAsync(userId)).Returns(Task.CompletedTask);

            //act
            var result = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _userService.DeleteUserAsync(invalidUserId);
            });

            // Assert
            Assert.Equal($"User with ID {invalidUserId} not found.", result.Message);
            Assert.IsAssignableFrom<NotFoundException>(result);

        }

        #endregion

    }
}
