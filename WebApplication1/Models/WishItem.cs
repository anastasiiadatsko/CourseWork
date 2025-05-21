using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class WishItem
    {
        public int Id { get; set; }
        [Display(Name = "Введіть своє бажання")]
        [Required(ErrorMessage = "Назва бажання обовʼязкова")]
        public string Title { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Сума має бути більшою за 0")]
        [Display(Name = "Потрібна сума")]
        public decimal TargetAmount { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Придбати до")]
        public DateTime? TargetDate
        {
            get => _targetDate;
            set => _targetDate = value.HasValue
                ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
                : null;
        }
        private DateTime? _targetDate;

        //[Display(Name = "Чи можливо придбати?")]
       // public bool IsAchievable { get; set; }

        [Display(Name = "Виконано")]
        public bool IsCompleted { get; set; }

       // [Display(Name = "Місяць")]
       // public string Month { get; set; }

       public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
