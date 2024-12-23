using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using api.Controllers;
using api.Models;
using api.Repositories;
using api.Services;
using api.Services.PostService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using MongoDB.Driver;
using Moq;
using Xunit.Sdk;

namespace TestProject1.Controllers
{
    public class PostControllerTest 
    {
        private readonly PostController _postController;
        private readonly Mock<IPostInterface> _postServiceMock = new Mock<IPostInterface>();
        //private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private Post post = new Post()
        {
            Id = "1",
            Content = "content",
            Date = DateTime.Now,
            Title = "title",
            UserId = "1"
        };
        List<Post> PostsList = new List<Post>();
        

        public PostControllerTest()
        {
            _postController = new PostController(_postServiceMock.Object);
        }

        [Fact(DisplayName = "Should return OkObjectResult  when posts exists")]
        public async void GetPosts_ShouldReturnAnOkActionResult_WhenPostsExists()
        {
            //Arrange
            var posts = PostsList;
            _postServiceMock.Setup(s => s.GetPosts()).ReturnsAsync(posts);

            // Act
            var actual = await _postController.GetPosts();
            var actionResult = Assert.IsType<ActionResult<ResponseDto<Post>>>(actual);
            var badRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var responseDto = Assert.IsType<ResponseDto<List<Post>>>(badRequestResult.Value);
            Assert.Equal(200, responseDto.Status);
            Assert.Equal(posts, responseDto.Data);
            Assert.Contains("Os posts foram encontrados", responseDto.Message);
            _postServiceMock.Verify(s => s.GetPosts(), Times.Once);
        }
        [Fact(DisplayName = "Should return an BadRequestObjectResult  when posts not exists")]
        public async void GetPosts_ShouldReturnAnBadRequest_WhenPostsNotExists()
        {
            //Arrange
            _postServiceMock.Setup(s => s.GetPosts()).ThrowsAsync(new Exception());
            // Act
            var actual = await _postController.GetPosts();
            //Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto<Post>>>(actual);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var responseDto = Assert.IsType<ResponseDto<List<Post>>>(badRequestResult.Value);
            Assert.Equal(404, responseDto.Status);
            Assert.Equal(new List<Post>(), responseDto.Data);
            Assert.Contains(new Exception().Message, responseDto.Message);
            _postServiceMock.Verify(s=>s.GetPosts(), Times.Once);

        }
        [Fact(DisplayName = "Should return a  OkObjectResult   when  posts exists")]
        public async void GetPostsById_ShouldReturnAPosts_WhenPostsExists()
        {
            //Arrange
            var id = post.Id;
            _postServiceMock.Setup(s => s.GetPostById(id)).ReturnsAsync(post);
            // Act
            var actual = await _postController.GetPostById(id);
            //Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto<Post>>>(actual);
            var badRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var responseDto = Assert.IsType<ResponseDto<Post>>(badRequestResult.Value);
           
            Assert.Equal(200, responseDto.Status);
            Assert.Equal(post.Title, responseDto.Data.Title);
            Assert.Contains("Post encontrado com sucesso", responseDto.Message);
            _postServiceMock.Verify(s => s.GetPostById(It.IsAny<string>()), Times.Once);
        }
        [Fact(DisplayName = "Should return  a BadRequestObjectResult   when  posts not  exists")]
        public async void GetPostById_ShouldReturnAnBadRequestObjectResult_WhenPostsNotExists()
        {
            //Arrange
            var id = "";
            _postServiceMock.Setup(s => s.GetPostById(It.IsAny<string>())).ThrowsAsync(new Exception());
            // Act
            var actual = await _postController.GetPostById(id);
            //Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto<Post>>>(actual);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var responseDto = Assert.IsType<ResponseDto<Post>>(badRequestResult.Value);

            Assert.Equal(404, responseDto.Status);
            Assert.Null(responseDto.Data.Id);
            Assert.Null(responseDto.Data.UserId);
            Assert.Null(responseDto.Data.Content);
            Assert.Equal(new DateTime(), responseDto.Data.Date);
            Assert.Null(responseDto.Data.Title);
            
            Assert.Contains(new Exception().Message, responseDto.Message);
            _postServiceMock.Verify(s => s.GetPostById(It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName ="Should return a created post with success when post is created")]
        public async void CreatePost_ShouldReturnAPost_WhenPostIsCreated()
        {
            //Arrange
            var userId = post.UserId;
            _postServiceMock.Setup(s => s.CreatePost(userId,post)).ReturnsAsync(post);
            // Act
            var actual=await _postController.CreatePost(post);
            //Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto<Post>>>(actual);
            var OkObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var responseDto = Assert.IsType<ResponseDto<Post>>(OkObjectResult.Value);

            Assert.Equal(201, responseDto.Status);
            Assert.Contains("Post criado com sucesso",responseDto.Message);
            Assert.Equal(post,responseDto.Data);
            Assert.Equal(post.Id, responseDto.Data.Id);
            Assert.Equal(post.Title, responseDto.Data.Title);
            Assert.Equal(post.Content, responseDto.Data.Content);
            Assert.Equal(post, responseDto.Data);
            
            

            Assert.Contains(new Exception().Message, responseDto.Message);
            _postServiceMock.Verify(s => s.CreatePost(It.IsAny<string>(),It.IsAny<Post>()), Times.Once);

        }

        [Fact(DisplayName ="Should return a BadRequestObjectResult when the post not is created")]
        public async void CreatePost_ShouldReturnAnBadRequestObjectResult()
        {
            //Arrange
            var userId = post.UserId;
            _postServiceMock.Setup(s => s.CreatePost(userId, post)).ThrowsAsync(new Exception());
            // Act
            var actual = await _postController.CreatePost(post);
            //Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto<Post>>>(actual);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var responseDto = Assert.IsType<ResponseDto<Post>>(badRequestObjectResult.Value);
            
            Assert.Equal(400, responseDto.Status);
            Assert.Contains("Post criado com sucesso", responseDto.Message);
            Assert.Equal(post, responseDto.Data);
            Assert.Equal(post.Id, responseDto.Data.Id);
            Assert.Equal(post.Title, responseDto.Data.Title);
            Assert.Equal(post.Content, responseDto.Data.Content);
            Assert.Equal(post, responseDto.Data);



            Assert.Contains(new Exception().Message, responseDto.Message);
            _postServiceMock.Verify(s => s.CreatePost(It.IsAny<string>(), It.IsAny<Post>()), Times.Once);
        }

        [Fact(DisplayName ="Should Return all posts with success when posts exists")]
        public async void GetPostsByUserId_ShouldReturnPosts_WhenPostsExists()
        {
            //Arrange
            var userId = post.UserId;
            PostsList.Add(post);
            _postServiceMock.Setup(s => s.GetPostsByUserId(userId)).ReturnsAsync(PostsList);
            // Act
            var actual = await _postController.GetPostsByUserId(userId);
            //Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto<List<Post>>>>(actual);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var responseDto = Assert.IsType<ResponseDto<List<Post>>>(okObjectResult.Value);

            Assert.Equal(200, responseDto.Status);
            Assert.Contains("Posts encontrados com sucesso", responseDto.Message);
            Assert.Equal(post, responseDto.Data[0]);
            Assert.Equal(post.Id, responseDto.Data[0].Id);
            Assert.Equal(post.Title, responseDto.Data[0].Title);
            Assert.Equal(post.Content, responseDto.Data[0].Content);
            Assert.Equal(post, responseDto.Data[0]);
            _postServiceMock.Verify(s => s.GetPostsByUserId(It.IsAny<string>()), Times.Once);
        }
        [Fact(DisplayName = "Should Return an NotFoundObjectResult  when posts  not exists")]
        public async void GetPostsByUserId_ShouldReturnNotFoundObjectResult_WhenPostsNotExists()
        {
            //Arrange
            var userId = "";
            
            _postServiceMock.Setup(s => s.GetPostsByUserId(It.IsAny<string>())).ThrowsAsync(new Exception());
            // Act
            var actual = await _postController.GetPostsByUserId(userId);
            //Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto<List<Post>>>>(actual);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            var responseDto = Assert.IsType<ResponseDto<List<Post>>>(notFoundObjectResult.Value);

            Assert.Contains("Os posts não foram encontrados", responseDto.Message);
            Assert.Equal(404, responseDto.Status);
            Assert.Empty(responseDto.Data);
            _postServiceMock.Verify(s => s.GetPostsByUserId(It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName ="Should return an OkObjectResult when post is updated with success")]
        public async void Put_ShouldReturnAnOkObjectResult_WhenPostIsUpdated()
        {
            //Arrange
            var id = post.Id;
         
            _postServiceMock.Setup(s => s.Put(id,post)).ReturnsAsync(post);
            // Act
            var actual = await _postController.Put(id,post);
            //Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto<Post>>>(actual);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var responseDto = Assert.IsType<ResponseDto<Post>>(okObjectResult.Value);

            Assert.Contains("O post foi  atualizado com sucesso", responseDto.Message);
            Assert.Equal(200, responseDto.Status);
            Assert.NotNull(responseDto.Data);
            Assert.Equal(post, responseDto.Data);
            Assert.Equal(post.Id, responseDto.Data.Id);
            Assert.Equal(post.Title, responseDto.Data.Title);
            Assert.Equal(post.Content, responseDto.Data.Content);
            _postServiceMock.Verify(s => s.Put(It.IsAny<string>(),It.IsAny<Post>()), Times.Once);
        }
        [Fact(DisplayName = "Should return an NotFoundObjectResult when post is not updated")]
        public async void Put_ShouldReturnAnNotFoundObjectResult_WhenPostIsNotUpdated()
        {
            //Arrange
            var id = post.Id;
            var postEmpty=new Post();
            _postServiceMock.Setup(s => s.Put(It.IsAny<string>(),It.IsAny<Post>())).ThrowsAsync(new Exception());
            // Act
            var actual = await _postController.Put(id, postEmpty);
            //Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto<Post>>>(actual);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            var responseDto = Assert.IsType<ResponseDto<Post>>(notFoundObjectResult.Value);

            Assert.Contains("O post não foi encontrado", responseDto.Message);
            Assert.Equal(404, responseDto.Status);
            Assert.NotNull(responseDto.Data);
            Assert.Equal(postEmpty.Id, responseDto.Data.Id);
            Assert.Equal(postEmpty.Title, responseDto.Data.Title);
            Assert.Equal(postEmpty.Content, responseDto.Data.Content);
            _postServiceMock.Verify(s => s.Put(It.IsAny<string>(), It.IsAny<Post>()), Times.Once);
        }

        [Fact(DisplayName ="Should return an NoContentResult when post is deleted with success")]
        public async void Delete_SouldReturnAnNoContentResult_whenPostIsDeleted()
        {
            //Arrange
            var id = post.Id;
            _postServiceMock.Setup(s => s.Delete(id)).ReturnsAsync(true);
            // Act
            var actual = await _postController.Delete(id);
            //Assert
            var actionResult = Assert.IsType<NoContentResult>(actual);
            Assert.Equal(204,actionResult.StatusCode);
            
            _postServiceMock.Verify(s => s.Delete(It.IsAny<string>()), Times.Once);
        }
        [Fact(DisplayName = "Should return an NotFoundObjectResult when post not exists")]
        public async void Delete_SouldReturnAnNotFoundObjectResult_whenPostNotExists()
        {
            //Arrange
            var id = "";
            _postServiceMock.Setup(s => s.Delete(It.IsAny<string>())).ThrowsAsync(new Exception());
            // Act
            var actual = await _postController.Delete(id);
            //Assert
            var actionResult = Assert.IsType<NotFoundObjectResult>(actual);
            var notFoundObjectResult = Assert.IsType<ResponseDto<string>>(actionResult.Value);
            Assert.Equal("O post não foi encontrado"+new Exception().Message, notFoundObjectResult.Message);
            Assert.Equal(404, actionResult.StatusCode);
            Assert.Equal(string.Empty, notFoundObjectResult.Data);
            _postServiceMock.Verify(s => s.Delete(It.IsAny<string>()), Times.Once);
        }
    }
}
