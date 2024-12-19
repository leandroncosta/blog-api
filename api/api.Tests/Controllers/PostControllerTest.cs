using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using api.Controllers;
using api.Models;
using api.Services;
using api.Services.PostService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using MongoDB.Driver;
using Moq;

namespace TestProject1.Controllers
{
    public class PostControllerTest 
    {
        private readonly PostController _postController;
        private readonly Mock<IPostInterface> _postServiceMock = new Mock<IPostInterface>();
       
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
        [Fact(DisplayName = "Should return an BadRequest  when posts not exists")]
        public async void GetPosts_ShouldReturnABadRequest_WhenPostsNotExists()
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
        public async void GetPostById_ShouldReturnABadRequest_WhenPostsNotExists()
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

        [Fact()]
        public async void CreatePost_ShouldReturnAPost_WhenPostExist()
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

        [Fact()]
        public void PostTest()
        {
            Assert.Fail();
        }

        [Fact()]
        public void getPostsByUserIdTest()
        {
            Assert.Fail();
        }

        [Fact()]
        public void PutTest()
        {
            Assert.Fail();
        }

        [Fact()]
        public void DeleteTest()
        {
            Assert.Fail();
        }
    }
}
