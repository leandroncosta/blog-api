using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    public record UpdateUserDto(string ?Username, string ?Password);

}

