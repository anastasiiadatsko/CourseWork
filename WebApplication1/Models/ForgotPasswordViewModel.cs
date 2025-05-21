using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Введіть email")]
        [EmailAddress(ErrorMessage = "Невірний email")]
        public string Email { get; set; } = string.Empty;
    }
}
