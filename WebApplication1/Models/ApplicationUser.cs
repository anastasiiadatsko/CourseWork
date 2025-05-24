using Microsoft.AspNetCore.Identity;
using System;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public string? ProfileImagePath { get; set; }
        public ICollection<Wallet> Wallets { get; set; }
        public ICollection<WishItem> WishItems { get; set; }
        public ICollection<BudgetCategory> BudgetCategories { get; set; }

    }
}
