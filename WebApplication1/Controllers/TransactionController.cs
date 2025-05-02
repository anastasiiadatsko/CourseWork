using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transaction/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.Date = DateTime.Now;
                transaction.Type = transaction.Amount > 0 ? TransactionType.Income : TransactionType.Expense;
                transaction.WalletId = 1; // тимчасово, поки не підключено користувача

                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Success));
            }

            return View(transaction);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
