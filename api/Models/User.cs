using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace api.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("username")]
        public string UserName { get; set; }

        [BsonElement("password")]
        public String Password { get; set; }


        //[BsonElement("posts")]
        //[JsonIgnore]
        //public List<Post> ? Posts { get; set; }
        [BsonElement("postsIds")]
        public List<ObjectId>? PostsIds { get; set; } // Lista de IDs dos Posts
    }
}
