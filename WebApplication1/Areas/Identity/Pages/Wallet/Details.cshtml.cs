using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Areas.Identity.Pages.Wallet
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Wallet Wallet { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id);

            if (Wallet == null)
                return NotFound();

            return Page();
        }
    }
}
