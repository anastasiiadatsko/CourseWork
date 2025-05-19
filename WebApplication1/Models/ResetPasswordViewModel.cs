using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть новий пароль")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Пароль має бути щонайменше 6 символів")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
