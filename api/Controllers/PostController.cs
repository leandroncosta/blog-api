using api.Models;
using api.Services;
using api.Services.PostService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
       private readonly IPostInterface _postInterface; // interface do service 

        public PostController(IPostInterface postInterface)
        {
            _postInterface = postInterface;
        }
        // GET: api/<PostController>
        [HttpGet]
        public  async Task<ActionResult<ResponseDto<Post>>> GetPosts()
        {
            try
            {
                var  posts = await  _postInterface.GetPosts();
                return Ok(new ResponseDto<Post>.Builder()
                        .SetStatus(200)
                        .SetMessage("Os posts foram encontrados")
                        .SetData(posts)
                        .Build<List<Post>>());
            }
            catch (Exception ex) {
                return BadRequest(new ResponseDto<Post>.Builder()
                         .SetStatus(404)
                         .SetMessage(ex.Message)
                         .SetData(new List<Post>())
                         .Build<List<Post>>());

            }
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public async  Task<ActionResult<ResponseDto<Post>>> GetPostById(string id)
        { 
            try
            {
                var post = await _postInterface.GetPostById(id);
                return Ok(new ResponseDto<Post>.Builder()
                    .SetStatus(200)
                    .SetMessage("Post encontrado com sucesso")
                    .SetData(post)
                    .Build<Post>());
            }
            catch (Exception ex) { 
                return BadRequest(
                    new ResponseDto<Post>.Builder()
                    .SetStatus(404)
                    .SetMessage(ex.Message)
                    .SetData(new Post())
                    .Build<Post>());
            }
        }

        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost([FromBody]Post post)
        {       
            var userId =  User.FindFirst("userId")?.Value;
            post.UserId = userId;
            try
            {
                var result=await _postInterface.CreatePost(userId,post);
                var response = new ResponseDto<Post>.Builder()
                    .SetStatus(201)
                    .SetMessage("Post criado com sucesso")
                    .SetData(result)
                    .Build<Post>();
                return CreatedAtAction(nameof(GetPosts), new { id = response.Data.Id }, response);
            }
            catch (Exception ex) {
                return BadRequest(new ResponseDto<Post>.Builder()
                    .SetStatus(400)
                    .SetMessage("Post não foi criado")
                    .SetData(post)
                    .Build<Post>());
            }
            }

        [HttpGet("GetPostsOfUser/{userId}")]
        public async Task<ActionResult<ResponseDto<List<Post>>>> GetPostsByUserId(string userId)
        {
            try
            {
                var posts = await _postInterface.GetPostsByUserId(userId);
                return Ok(new ResponseDto<List<Post>>.Builder()
                    .SetMessage("Posts encontrados com sucesso")
                    .SetStatus(200)
                    .SetData(posts)
                    .Build<List<Post>>());
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseDto<Post>.Builder()
                    .SetMessage("O posts não foram encontrados" + ex.Message)
                    .SetStatus(404)
                    .SetData(string.Empty)
                    .Build<Post>());
            }
        }
        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<Post>>> Put(string id, [FromBody] Post post)
        {
            try
            {
                var postDb =await _postInterface.Put(id, post);
                return Ok(new ResponseDto<Post>.Builder()
                   .SetMessage("O post foi  atualizado com sucesso")
                   .SetStatus(200)
                   .SetData(post)
                   .Build<Post>());
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseDto<Post>.Builder()
                   .SetMessage("O post não foi encontrado" + ex.Message)
                   .SetStatus(404)
                   .SetData(string.Empty)
                   .Build<Post>());
            }
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var postDb = await _postInterface.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseDto<Post>.Builder()
                   .SetMessage("O post não foi encontrado" + ex.Message)
                   .SetStatus(204)
                   .SetData(string.Empty)
                   .Build<Post>());
            }
        }
    }
}
