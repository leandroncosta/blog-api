using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
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
            var posts = await _postsCollection.Find(_ => true).ToListAsync();
            return Ok(posts);
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var objectId= new ObjectId(id);
            var post =await  _postsCollection.Find(p => p.Id==objectId).FirstOrDefaultAsync();
            return Ok(post);
        }

        // POST api/<PostController>
        [HttpPost]
        public async Task<ActionResult<Post>> Post(Post post)
        {
            await _postsCollection.InsertOneAsync(post);
            return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
        }

        [HttpGet("GetPostsOfUser/{userId}")]
        public async Task<IActionResult> getPostsByUserId(string userId)
        {
            var posts = await _postsCollection.Find(p => p.UserId== userId).ToListAsync();
            return Ok(posts);
        }
        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult>  Put(string id, [FromBody] Post post)
        {
            var objectId = new ObjectId(id);
            var postDb =   _postsCollection.Find(p => p.Id == objectId).FirstOrDefault();
            // Se o post não for encontrado, retornar 404
            if (postDb == null)
            {
                return NotFound();
            }
            postDb.Title = post.Title;
            postDb.Content = post.Content;
           await _postsCollection.ReplaceOneAsync(p=> p.Id== objectId, postDb);
            return Ok(post);
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
