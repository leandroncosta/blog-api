using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("username")]
        public string UserName { get; set; }

        [BsonElement("password")]
        public String Password { get; set; }


        //[BsonElement("posts")]
        //[JsonIgnore]
        //public List<Post> ? Posts { get; set; }
        [BsonElement("postsIds")]
        public List<string>? PostsIds { get; set; } // Lista de IDs dos Posts
    }
}