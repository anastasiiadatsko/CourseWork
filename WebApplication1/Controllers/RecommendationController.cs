using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class RecommendationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecommendationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Групуємо витрати по категоріях
            var categoryData = await _context.Transactions
                .Where(t => t.Type == TransactionType.Expense)
                .GroupBy(t => t.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Total = g.Sum(t => t.Amount)
                })
                .OrderByDescending(g => g.Total)
                .ToListAsync();

            var topCategory = categoryData.FirstOrDefault();

            var recommendations = new List<string>();

            if (topCategory != null)
            {
                recommendations.Add($"Витрати на категорію \"{topCategory.Category}\" є найвищими ({topCategory.Total} грн). Розгляньте можливість зменшити витрати в цій категорії.");
            }

            recommendations.Add("Рекомендується заощаджувати 20% доходу щомісяця.");
            recommendations.Add("Створіть місячний бюджет і регулярно його перевіряйте.");

            ViewBag.Recommendations = recommendations;

            return View();
        }
    }
}
