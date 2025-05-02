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

        public decimal Amount { get; set; }

        public string Category { get; set; } // їжа, розваги, тощо

        public TransactionType Type { get; set; }

        public DateTime Date { get; set; }

        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
    }
}
