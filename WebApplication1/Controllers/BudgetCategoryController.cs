using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class BudgetCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BudgetCategoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);

            ViewBag.Categories = await _context.Transactions
                .Include(t => t.Wallet)
                .Where(t => t.Wallet.ApplicationUserId == user.Id && t.Type == TransactionType.Expense)
                .Select(t => t.Category)
                .Distinct()
                .ToListAsync();

            ViewBag.Wallets = await _context.Wallets
                .Where(w => w.ApplicationUserId == user.Id)
                .ToListAsync();

            return View();
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var categories = await _context.BudgetCategories
                .Where(c => c.ApplicationUserId == user.Id)
                .Include(c => c.Wallet)
                .ToListAsync();

            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BudgetCategory model)
        {
            var user = await _userManager.GetUserAsync(User);

            // 🔥 Признач до валідації!
            model.ApplicationUserId = user.Id;

            if (ModelState.IsValid)
            {
                _context.BudgetCategories.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // 🔁 Навіть якщо форма невалідна — дані не зникнуть
            ViewBag.Categories = await _context.Transactions
                .Include(t => t.Wallet)
                .Where(t => t.Wallet.ApplicationUserId == user.Id && t.Type == TransactionType.Expense)
                .Select(t => t.Category)
                .Distinct()
                .ToListAsync();

            ViewBag.Wallets = await _context.Wallets
                .Where(w => w.ApplicationUserId == user.Id)
                .ToListAsync();

            return View(model);
        }

    }
}