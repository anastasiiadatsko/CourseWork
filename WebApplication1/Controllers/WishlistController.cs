using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishlistController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Wishlist
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var items = await _context.WishItems
                .Where(w => w.ApplicationUserId == userId)
                .ToListAsync();

            return View(items);
        }

        // GET: Wishlist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wishlist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WishItem item)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                item.ApplicationUserId = userId;
                item.IsAffordable = false;

                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }
    }
}
