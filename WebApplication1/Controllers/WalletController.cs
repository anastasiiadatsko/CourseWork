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

        // GET: /Wallet/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Wallet/Create
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

            // ✅ Перехід до сторінки перегляду створеного гаманця
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
            var userId = _userManager.GetUserId(User);
            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.Id == id && w.ApplicationUserId == userId);

            if (wallet == null)
                return NotFound();

            return View(wallet);
        }


    }
}
