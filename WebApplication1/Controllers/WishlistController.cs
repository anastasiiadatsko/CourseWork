using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class WishListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishListController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? month)
        {
            var user = await _userManager.GetUserAsync(User);
            var wishesQuery = _context.WishItems.Where(w => w.ApplicationUserId == user.Id);

            if (!string.IsNullOrEmpty(month))
            {
                // Отримуємо номер місяця з назви (укр)
                var culture = System.Globalization.CultureInfo.GetCultureInfo("uk-UA");
                var monthIndex = Array.FindIndex(
                    culture.DateTimeFormat.MonthNames,
                    m => string.Equals(m, month, StringComparison.OrdinalIgnoreCase)
                ) + 1;

                if (monthIndex > 0)
                {
                    wishesQuery = wishesQuery.Where(w =>
                        w.TargetDate.HasValue && w.TargetDate.Value.Month == monthIndex
                    );
                }
            }

            ViewBag.Month = month;
            var list = await wishesQuery.OrderBy(w => w.TargetDate).ToListAsync();
            return View(list);
        }


        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WishItem item)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                item.ApplicationUserId = user.Id;
                item.CreatedAt = DateTime.UtcNow;

                if (item.TargetDate.HasValue)
                    item.TargetDate = DateTime.SpecifyKind(item.TargetDate.Value, DateTimeKind.Utc);

                _context.WishItems.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _context.WishItems.FindAsync(id);
            if (item == null || item.ApplicationUserId != userId)
                return Unauthorized();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WishItem updated)
        {
            if (id != updated.Id)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            var existing = await _context.WishItems.FindAsync(id);
            if (existing == null || existing.ApplicationUserId != userId)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                existing.Title = updated.Title;
                existing.TargetAmount = updated.TargetAmount;
                existing.Month = updated.Month;
                existing.IsAchievable = updated.IsAchievable;
                existing.IsCompleted = updated.IsCompleted;

                if (updated.TargetDate.HasValue)
                    existing.TargetDate = DateTime.SpecifyKind(updated.TargetDate.Value, DateTimeKind.Utc);
                else
                    existing.TargetDate = null;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(updated);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _context.WishItems.FindAsync(id);
            if (item == null || item.ApplicationUserId != userId)
                return Unauthorized();

            return View(item);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _context.WishItems.FindAsync(id);
            if (item == null || item.ApplicationUserId != userId)
                return Unauthorized();

            _context.WishItems.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}