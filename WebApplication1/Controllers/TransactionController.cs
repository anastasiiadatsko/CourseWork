using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Transaction/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var wallets = await _context.Wallets
                .Where(w => w.ApplicationUserId == userId)
                .ToListAsync();

            var transactions = await _context.Transactions
                .Include(t => t.Wallet)
                .Where(t => t.Wallet.ApplicationUserId == userId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            ViewBag.Wallets = wallets;
            ViewBag.Categories = new List<string> { "Їжа", "Транспорт", "Відпочинок", "Розваги", "Інше" };
            ViewBag.Transactions = transactions;

            return View(new Transaction());
        }

        // POST: Transaction/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Transaction transaction)
        {
            var userId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                transaction.Date = DateTime.UtcNow;

                var wallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.Id == transaction.WalletId && w.ApplicationUserId == userId);

                if (wallet == null)
                {
                    ModelState.AddModelError("", "Обраний гаманець не знайдено.");
                }
                else
                {
                    // Оновлення балансу:
                    if (transaction.Type == TransactionType.Expense)
                    {
                        wallet.Balance -= transaction.Amount;
                    }
                    else if (transaction.Type == TransactionType.Income)
                    {
                        wallet.Balance += transaction.Amount;
                    }

                    _context.Transactions.Add(transaction);
                    _context.Wallets.Update(wallet); // збереження оновленого балансу
                    await _context.SaveChangesAsync();

                    ModelState.Clear();
                    return RedirectToAction(nameof(Index));
                }
            }

            // Повторна передача ViewBag'ів у разі помилки
            ViewBag.Wallets = await _context.Wallets
                .Where(w => w.ApplicationUserId == userId)
                .ToListAsync();

            ViewBag.Categories = new List<string> { "Їжа", "Транспорт", "Відпочинок", "Розваги", "Інше" };

            ViewBag.Transactions = await _context.Transactions
                .Include(t => t.Wallet)
                .Where(t => t.Wallet.ApplicationUserId == userId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            return View(transaction);
        }
    }
}
