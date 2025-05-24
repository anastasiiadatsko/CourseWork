using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Services
{
    public class RecommendationService
    {
        private readonly IEmailSender _emailSender;

        public RecommendationService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<List<string>> GenerateRecommendationsAsync(ApplicationUser user, List<Transaction> expenses, List<BudgetCategory> categories)
        {
            var recommendations = new List<string>();

            foreach (var category in categories)
            {
                var spent = expenses
                    .Where(e => e.Category.Equals(category.CategoryName, StringComparison.OrdinalIgnoreCase))
                    .Sum(e => e.Amount);

                if (spent > category.LimitAmount)
                {
                    string msg = $"⚠️ Ви перевищили встановлений вами ліміт у категорії '{category.CategoryName}'. Витрати: {spent:F0} грн / Ліміт: {category.LimitAmount:F0} грн.";
                    recommendations.Add(msg);

                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        await _emailSender.SendEmailAsync(user.Email,
                            $"Перевищено ліміт у категорії {category.CategoryName}",
                            $"<p>{msg}</p><p>Перевірте та оновіть свої бюджетні налаштування у профілі.</p>");
                    }
                }
            }

            var groupedExpenses = expenses
                .GroupBy(t => t.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) })
                .OrderByDescending(g => g.Total)
                .ToList();

            foreach (var cat in groupedExpenses)
            {
                bool alreadyRecommended = categories.Any(c => c.CategoryName.Equals(cat.Category, StringComparison.OrdinalIgnoreCase));

                if (!alreadyRecommended && cat.Total > 1000)
                {
                    string msg = $"📌 У вас високі витрати в категорії '{cat.Category}' — {cat.Total:F0} грн. Розгляньте можливість встановити ліміт у профілі.";
                    recommendations.Add(msg);

                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        await _emailSender.SendEmailAsync(user.Email,
                            $"Рекомендація по витратах: {cat.Category}",
                            $"<p>{msg}</p><p>Додайте цю категорію у свій бюджет, щоб відстежувати її у майбутньому.</p>");
                    }
                }
            }

            if (recommendations.Count == 0)
            {
                recommendations.Add("✅ Усі витрати в межах встановлених вами лімітів. Продовжуйте в тому ж дусі!");
            }

            return recommendations;
        }
    }
}
