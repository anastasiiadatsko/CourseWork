using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Некоректний email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запамʼятати мене")]
        public bool RememberMe { get; set; }
    }
}
