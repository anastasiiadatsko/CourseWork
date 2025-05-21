using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public enum TransactionType
    {
        Income,
        Expense
    }
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Сума має бути більшою за 0")]
        public decimal Amount { get; set; }

        [Required]
        public string Category { get; set; }

        [Required(ErrorMessage = "Оберіть тип транзакції")]
        public TransactionType Type { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public int WalletId { get; set; }
        public Wallet? Wallet { get; set; }
    }
}
