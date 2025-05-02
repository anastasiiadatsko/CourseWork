namespace WebApplication1.Models // або FinanceTracker.Models, якщо твій проект так називається
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime RegisteredAt { get; set; }
    }
}
