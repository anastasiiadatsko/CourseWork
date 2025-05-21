using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Wallet
    {
        public int Id { get; set; }

        [Required]
        public string BankName { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string OwnerInitials { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        // FK до користувача
        [Required]
        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
