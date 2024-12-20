using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.Controllers;
using api.Models;
using api.Repositories;
using api.Services;
using api.Services.PostService;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Moq;

namespace TestProject1.ServicesTest
{
    public class PostServiceTest 
    {

        private readonly PostService _postService;
        private readonly IUserService _userService;
        private readonly Mock<IPostRepository> _postRepositoryMock=new Mock<IPostRepository>();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        public PostServiceTest()
        {
            _postService = new PostService(_postRepositoryMock.Object, _userService, _userRepositoryMock.Object);

        }
        private Post Post = new Post()
        {
            Id = "1",
            UserId = "1",
            Content = "content",
            Date = DateTime.Now,
            Title = "title",
            
        };
        private Post Post2 = new Post()
        {
            Id = "2",
            UserId = "2",
            Content = "content2",
            Date = DateTime.Now,
            Title = "title2",

        };
        List<Post> PostsList = new List<Post>();
        
      
        [Fact(DisplayName = "Should return all posts with success when posts exists ")]
        public async void GetPosts_ShouldReturnAllPosts_WhenPostsExists()
        {
            //Arrange
            PostsList.Add(Post);
            var expected = PostsList[0];

          
            _postRepositoryMock.Setup(p => p.GetPosts()).ReturnsAsync(PostsList);
            //Act 
            var actual = await _postService.GetPosts();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(PostsList.Count, actual.Count);
            Assert.Equal(expected.Id, actual[0].Id);
            Assert.Equal(expected.UserId, actual[0].UserId);
            Assert.Equal(expected.Title, actual[0].Title);
            Assert.Equal(expected.Date, actual[0].Date);
            Assert.Equal(expected.Content, actual[0].Content);
            _postRepositoryMock.Verify(p=> p.GetPosts(),Times.Once);
        }
        [Fact(DisplayName = "Should return a empty list  when posts not exists")]
        public async void GetPosts_ShouldReturnAemptyList_WhenPostsNotExists()
        {
            //Arrange
            PostsList.ToList();
            
            _postRepositoryMock.Setup(p => p.GetPosts()).ReturnsAsync(PostsList);
            //Act 
            var actual = await _postService.GetPosts();
            await _postService.GetPosts();
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            _postRepositoryMock.Verify(p => p.GetPosts(), Times.Exactly(2));

        }

        [Fact(DisplayName ="Should return a post when is created")]
        public async void CreatePost_ShouldReturnThePost_WhenPostIsCreated()
        {
            //Arrange
            var userId = Post.UserId;
            var post = Post;
            var user=new User { Id = "1", UserName = "User1" };
            _userRepositoryMock.Setup(userRepository => userRepository.GetByIdAsync(userId)).ReturnsAsync(user);
            _postRepositoryMock.Setup(p => p.CreatePost(userId,post)).ReturnsAsync(post);
            //Act
            var actual = await _postService.CreatePost(userId,post);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(Post.Id, actual.Id);
            Assert.Equal(userId, actual.UserId);
            Assert.Equal(Post, actual);
            Assert.Equal(Post.Title, actual.Title);
            Assert.Equal(Post.Content, actual.Content);
            Assert.Equal(Post.Date, actual.Date);
            _postRepositoryMock.Verify((p) => p.CreatePost(userId, post),Times.Once);
        }

        [Fact(DisplayName = " Should throw an exception when the post is not created")]
        public async void CreatePost_ShouldThrowAnException_WhenPostIsNotCreated()
        {
            //Arrange
            var userId =Post.UserId;
            var post = Post;
            
            _postRepositoryMock.Setup(p => p.CreatePost(It.IsAny<string>(), It.IsAny<Post>())).ThrowsAsync(new Exception());
            
            // Act & Assert
           
            var exception =await Assert.ThrowsAsync<Exception>(() =>  _postService.CreatePost(userId, Post));
            Assert.IsType<Exception>(exception);
            Assert.NotNull(exception.Message);
            _postRepositoryMock.Verify(p => p.CreatePost(It.IsAny<string>(),It.IsAny<Post>()), Times.Once);

        }

        [Fact(DisplayName ="Should Return all posts of user With success")]
        public async void GetPostsByUserId_SloudReturnAllPostsOfUser_WhenPostsExists()
        {
            //Arrange
            PostsList.Add(Post);
            PostsList.Add(Post2);
            var userId = Post.UserId;
            var userId2 = Post2.UserId;
            _postRepositoryMock.Setup(p => p.GetPostsByUserId(userId)).ReturnsAsync(PostsList);
            //Act
            var actual =await  _postService.GetPostsByUserId(userId);
          
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(PostsList.Count,actual.Count);
            Assert.Equal(userId, actual[0].UserId);
            Assert.Equal(userId2, actual[1].UserId);
            _postRepositoryMock.Verify(p => p.GetPostsByUserId(userId), Times.Once);

        }
        [Fact(DisplayName = "Should throw an exception when the posts not exists")]
        public async void GetPostsByUserId_SloudThowsAnException_WhenPostsNotExists()
        {
            //Arrange
            _postRepositoryMock.Setup(p => p.GetPostsByUserId(It.IsAny<string>())).ThrowsAsync(new Exception());
            //Act & Assert
            var exception=await Assert.ThrowsAsync<Exception>(()=>_postService.GetPostsByUserId(""));
            Assert.IsType<Exception>(exception);
            _postRepositoryMock.Verify(p => p.GetPostsByUserId(It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName ="Should update and return a post with success")]
        public async void UpdatePost_ShouldReturnTheSuccessfullyUpdatedPost_WhenPostExists()
        {
            //Arrange
            var id=Post.Id;
            var post = Post;
            _postRepositoryMock.Setup(p => p.Put(id, post)).ReturnsAsync(Post);

            //Act
            var actual= await _postService.Put(id, post);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(post.Id, actual.Id);
            Assert.Equal(post.UserId, actual.UserId);
            Assert.Equal(post.Title, actual.Title);
            Assert.Equal(post.Content, actual.Content);
            Assert.Equal(post.Date, actual.Date);
            _postRepositoryMock.Verify(p=> p.Put(id,post),Times.Once);
        }
        [Fact(DisplayName = "Should throw  an exception when the post not exists")]
        public async void UpdatePost_ShouldThrowAnException_WhenPostNotExists()
        {
            //Arrange
            var post = Post;
            _postRepositoryMock.Setup(p => p.Put(It.IsAny<string>(),It.IsAny<Post>())).ThrowsAsync(new Exception());
            //Act && Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _postService.Put("",post));
            Assert.IsType<Exception>(exception);
            _postRepositoryMock.Verify(p => p.Put(It.IsAny<string>(),It.IsAny<Post>()), Times.Once);
        }
        
        [Fact(DisplayName ="Should delete a post with success")]
        public async void Delete_ShouldReturnTrue_WhenPostIsDeletedWithSuccess()
        {
            //Arrange
            var id = Post.Id;
            _postRepositoryMock.Setup(p =>p.Delete(id)).ReturnsAsync(true);
            //Act
            var actual= await _postService.Delete(id);
            //Assert
            Assert.True(actual);
            _postRepositoryMock.Verify(p=>p.Delete(id),Times.Once);
        }

        [Fact(DisplayName = "Should return false when post found ")]
        public async void Delete_ShouldReturnFalse_WhenNotFound()
        {
            //Arrange
            var id = Post.Id;
            _postRepositoryMock.Setup(p => p.Delete(It.IsAny<string>())).ReturnsAsync(false);
            //Act
            var actual = await _postService.Delete("");
            //Assert
            Assert.False(actual);
            _postRepositoryMock.Verify(p => p.Delete(""), Times.Once);
        }
    }
}
