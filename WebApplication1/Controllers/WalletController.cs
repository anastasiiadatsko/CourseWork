using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WalletController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WalletCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = _userManager.GetUserId(User);

            var wallet = new Wallet
            {
                BankName = model.BankName,
                Color = model.Color,
                Currency = model.Currency,
                CardNumber = model.CardNumber,
                OwnerInitials = model.OwnerInitials,
                Balance = model.Balance,
                ApplicationUserId = userId
            };

            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = wallet.Id });
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var wallets = await _context.Wallets
                .Where(w => w.ApplicationUserId == userId)
                .ToListAsync();

            return View(wallets);
        }

        public async Task<IActionResult> Details(int id)
        {
            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet == null)
                return NotFound();

            return View(wallet);
        }

        // Редагування (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet == null)
                return NotFound();

            return View(wallet);
        }

        // Редагування (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Wallet wallet)
        {
            if (id != wallet.Id)
            {
                Console.WriteLine("⚠️ ID у URL не співпадає з Wallet.Id");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        Console.WriteLine($"❌ {key}: {error.ErrorMessage}");
                    }
                }

                return View(wallet);
            }

            try
            {
                _context.Update(wallet);
                await _context.SaveChangesAsync();

                Console.WriteLine($"✅ Гаманець оновлено. ID: {wallet.Id}");
                return RedirectToAction("Details", new { id = wallet.Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Wallets.Any(e => e.Id == wallet.Id))
                {
                    Console.WriteLine($"❌ Не знайдено гаманець із ID: {wallet.Id}");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
