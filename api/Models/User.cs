using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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

        [BsonElement("posts")]
        public List<Post> Posts { get; set; }
    }
}
