using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly  IMongoCollection<Post> _postsCollection;

        public PostController(MongoDbService mongoDbService)
        {
            _postsCollection = mongoDbService.GetCollection<Post>("Post");
        }
        // GET: api/<PostController>
        [HttpGet]
        public async Task <ActionResult> GetPosts()
        {
            try
            {
                var posts = await _postsCollection.Find(_ => true).ToListAsync();
                return Ok(new ResponseDto<List<Post>>.Builder()
                    .SetStatus(200)
                    .SetMessage("Posts encontrados com sucesso")
                    .SetData(posts)
                    .Build<Post>());
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var objectId = new ObjectId(id);
                var post = await _postsCollection.Find(p => p.Id == objectId).FirstOrDefaultAsync();
                return Ok(new ResponseDto<Post>.Builder()
                    .SetStatus(200)
                    .SetMessage("Post encontrado com sucesso")
                    .SetData(post)
                    .Build<Post>());
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        // POST api/<PostController>
        [HttpPost]
        public async Task<ActionResult<Post>> Post(Post post)
        {
            try{
                await _postsCollection.InsertOneAsync(post);
                return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            }

        [HttpGet("GetPostsOfUser/{userId}")]
        public async Task<IActionResult> getPostsByUserId(string userId)
        {
            try
            { 
                var objetId = new ObjectId(userId);
                var posts = await _postsCollection.Find(p => p.UserId == objetId).ToListAsync();
                return Ok(posts);
            }
            catch (Exception ex) { 
                return NotFound(new ResponseDto<Post>.Builder()
                    .SetMessage("O post não foi encontrado"+ex.Message)
                    .SetStatus(404)
                    .SetData(string.Empty)
                    .Build<Post>());
            }
        }
        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult>  Put(ObjectId id, [FromBody] Post post)
        {

            try
            {
    
                var postDb = _postsCollection.Find(p => p.Id.Equals(id)).FirstOrDefault();
                if (postDb == null)
                {
                    return NotFound();
                }
                postDb.Title = post.Title;
                postDb.Content = post.Content;
                await _postsCollection.ReplaceOneAsync(p => p.Id.Equals(id), postDb);
                return Ok(post);
            }
            catch (Exception ex) { 
                 return NotFound(new ResponseDto<Post>.Builder()
                    .SetMessage("O post não foi encontrado"+ex.Message)
                    .SetStatus(404)
                    .SetData(string.Empty)
                    .Build<Post>());
            }
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        public async  Task<ActionResult> Delete(string id)
        {
            var objectId = new ObjectId(id);
            var postDb= _postsCollection.Find(p=> p.Id == objectId).FirstOrDefaultAsync();
            // Se o post não for encontrado, retornar 404
            if (postDb == null)
            {
                return NotFound();
            }
            await _postsCollection.DeleteOneAsync(p=> p.Id == objectId);
            return NoContent();

        }
    }
}
