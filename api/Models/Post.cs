using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace api.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public ObjectId UserId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

    }

}