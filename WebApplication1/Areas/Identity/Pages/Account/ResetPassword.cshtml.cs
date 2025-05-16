using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;

namespace WebApplication1.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ResetPasswordModel> _logger;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager, ILogger<ResetPasswordModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public ResetPasswordViewModel Input { get; set; } = new();

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
                return BadRequest("Код обовʼязковий");

            Input.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                _logger.LogWarning("⚠️ Користувач із email {Email} не знайдений", Input.Email);
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("🔑 Пароль змінено для {Email}", user.Email);
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                _logger.LogError("❌ Помилка скидання паролю: " + error.Description);
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
