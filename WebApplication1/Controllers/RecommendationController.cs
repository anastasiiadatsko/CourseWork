using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class RecommendationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RecommendationService _recommendationService;

        public RecommendationController(
            ApplicationDbContext context,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager,
            RecommendationService recommendationService)
        {
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
            _recommendationService = recommendationService;
        }

        // 📊 Головна сторінка рекомендацій
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Отримуємо бюджетні категорії та витрати
            var categories = await _context.BudgetCategories
                .Where(c => c.ApplicationUserId == user.Id)
                .ToListAsync();

            var expenses = await _context.Transactions
                .Include(t => t.Wallet)
                .Where(t => t.Type == TransactionType.Expense && t.Wallet.ApplicationUserId == user.Id)
                .ToListAsync();

            // 🔄 Додаємо відсутні категорії на основі транзакцій
            var existingNames = categories.Select(c => c.CategoryName.ToLower()).ToList();
            var newExpenseCategories = expenses
                .Select(e => e.Category)
                .Distinct()
                .Where(cat => !existingNames.Contains(cat.ToLower()))
                .ToList();

            foreach (var newCat in newExpenseCategories)
            {
                var walletId = expenses.First(e => e.Category == newCat).WalletId;

                _context.BudgetCategories.Add(new BudgetCategory
                {
                    CategoryName = newCat,
                    ApplicationUserId = user.Id,
                    BudgetPercentage = 0,
                    LimitAmount = 0,
                    WalletId = walletId
                });
            }

            if (newExpenseCategories.Any())
            {
                await _context.SaveChangesAsync();
                // Оновлюємо список після додавання
                categories = await _context.BudgetCategories
                    .Where(c => c.ApplicationUserId == user.Id)
                    .ToListAsync();
            }

            // 📬 Генерація рекомендацій
            var recommendations = await _recommendationService.GenerateRecommendationsAsync(user, expenses, categories);
            return View(recommendations);
        }

        // ⚙️ GET: Встановлення лімітів
        [HttpGet]
        public async Task<IActionResult> SetLimits()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var categories = await _context.BudgetCategories
                .Where(c => c.ApplicationUserId == user.Id)
                .ToListAsync();

            return View(categories);
        }

        // 💾 POST: Збереження лімітів
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetLimits(List<BudgetCategory> updatedCategories)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            foreach (var updated in updatedCategories)
            {
                var existing = await _context.BudgetCategories
                    .FirstOrDefaultAsync(c => c.Id == updated.Id && c.ApplicationUserId == user.Id);

                if (existing != null)
                {
                    existing.LimitAmount = updated.LimitAmount;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
