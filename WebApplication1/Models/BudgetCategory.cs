using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class BudgetCategory
    {
        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Range(0, 100)]
        public double BudgetPercentage { get; set; }

        [Range(0, double.MaxValue)]
        public decimal LimitAmount { get; set; }

        [Required]
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
