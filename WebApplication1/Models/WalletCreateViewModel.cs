using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class WalletCreateViewModel
    {
        [Required]
        public string BankName { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        [Display(Name = "Номер карти")]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Ініціали власника")]
        public string OwnerInitials { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Залишок")]
        public decimal Balance { get; set; }
    }
}
