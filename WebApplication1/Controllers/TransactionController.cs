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
                var userId = _userManager.GetUserId(User); // string

                transaction.Date = DateTime.Now;
                transaction.Type = transaction.Amount > 0 ? TransactionType.Income : TransactionType.Expense;

                // TODO: якщо реалізуєш Wallet, знайди гаманець користувача тут
                // transaction.WalletId = await _context.Wallets.Where(w => w.ApplicationUserId == userId).Select(w => w.Id).FirstOrDefaultAsync();

                transaction.WalletId = 1; // тимчасово, поки Wallet не реалізований

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
