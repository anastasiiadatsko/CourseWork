using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AnalyticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnalyticsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Групування витрат по місяцях
            var data = await _context.Transactions
                .Where(t => t.Type == TransactionType.Expense)
                .GroupBy(t => t.Date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(t => t.Amount)
                })
                .ToListAsync();

            var monthLabels = data.OrderBy(d => d.Month).Select(d => GetMonthName(d.Month)).ToList();
            var amounts = data.OrderBy(d => d.Month).Select(d => d.Total).ToList();

            ViewBag.Labels = monthLabels;
            ViewBag.Amounts = amounts;

            return View();
        }

        private string GetMonthName(int month)
        {
            return new DateTime(1, month, 1).ToString("MMMM");
        }

        [HttpGet]
        public async Task<IActionResult> Weekly(DateTime? startDate, DateTime? endDate, int? walletId)
        {
            var userId = _userManager.GetUserId(User);
            var wallets = await _context.Wallets
                .Where(w => w.ApplicationUserId == userId)
                .ToListAsync();

            ViewBag.Wallets = wallets;

            if (startDate == null || endDate == null || walletId == null)
            {
                ViewBag.Labels = new List<string>();
                ViewBag.Amounts = new List<decimal>();
                ViewBag.Categories = new List<string>();
                ViewBag.CategoryAmounts = new List<decimal>();
                return View();
            }

            var start = DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(endDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);

            var filteredTransactions = await _context.Transactions
                .Where(t => t.Type == TransactionType.Expense &&
                            t.WalletId == walletId &&
                            t.Date >= start && t.Date <= end)
                .ToListAsync();

            // По днях
            var dailyData = filteredTransactions
                .GroupBy(t => t.Date.ToString("yyyy-MM-dd"))
                .Select(g => new
                {
                    Day = g.Key,
                    Total = g.Sum(t => t.Amount)
                })
                .OrderBy(g => g.Day)
                .ToList();

            // По категоріях
            var categoryData = filteredTransactions
                .GroupBy(t => t.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Total = g.Sum(t => t.Amount)
                })
                .OrderByDescending(g => g.Total)
                .ToList();

            ViewBag.Labels = dailyData.Select(d => d.Day).ToList();
            ViewBag.Amounts = dailyData.Select(d => d.Total).ToList();
            ViewBag.Categories = categoryData.Select(c => c.Category).ToList();
            ViewBag.CategoryAmounts = categoryData.Select(c => c.Total).ToList();

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SelectedWalletId = walletId;

            return View();
        }
    }
}
