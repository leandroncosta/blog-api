namespace api.Models
{
    public class UserDTO
    {
        public string Id { get; set; }
        public String UserName { get; set; }
        public List<String> PostsId { get; set; }

        public static UserDTO ConvertToUserDto(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                PostsId = user.PostsIds
            };
        }

    };

}
