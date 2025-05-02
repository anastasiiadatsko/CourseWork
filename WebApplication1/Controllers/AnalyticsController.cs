using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AnalyticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsController(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
