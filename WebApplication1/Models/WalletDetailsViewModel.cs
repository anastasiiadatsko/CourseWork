using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class WalletDetailsViewModel
    {
        public Wallet Wallet { get; set; }

        public List<Transaction> RecentTransactions { get; set; }

        // Для графіка
        public List<string> Labels { get; set; } = new();
        public List<decimal> Amounts { get; set; } = new();
    }
}
