namespace WebApplication1.Models
{
    public class WishItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal DesiredAmount { get; set; }

        public bool IsAffordable { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
