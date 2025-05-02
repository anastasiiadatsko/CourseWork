using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Wishlist
        public async Task<IActionResult> Index()
        {
            var items = await _context.WishItems.ToListAsync();
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
                item.IsAffordable = false; // Поки без перевірки бюджету
                item.UserId = 1; // тимчасово, поки не реалізовано автентифікацію

                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }
    }
}
