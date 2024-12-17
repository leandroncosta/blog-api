namespace api.Models
{
    public class CreatePostDto
    {
        public string UserId { get; set; } 
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
