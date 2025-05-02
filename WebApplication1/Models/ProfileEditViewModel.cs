using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ProfileEditViewModel
    {
        [Required(ErrorMessage = "Ім'я є обов'язковим")]
        [StringLength(100, ErrorMessage = "Ім'я не може перевищувати 100 символів.")]
        public string Name { get; set; }

        public string? ExistingImagePath { get; set; }
    }
}
