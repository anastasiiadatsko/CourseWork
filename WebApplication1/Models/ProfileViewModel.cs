namespace WebApplication1.Models
{
    public class ProfileViewModel
    {
        public string Name { get; set; } 
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string? ImagePath { get; set; }
    }
}
