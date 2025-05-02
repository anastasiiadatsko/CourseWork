using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using WebApplication1.Models;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ConfirmEmailModel> _logger;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ILogger<ConfirmEmailModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                _logger.LogWarning("❌ Відсутній userId або code в запиті.");
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("❌ Користувач не знайдений з ID: {UserId}", userId);
                return NotFound($"Не знайдено користувача з ID '{userId}'.");
            }

            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

            if (!result.Succeeded)
            {
                _logger.LogError("❌ Помилка при підтвердженні email для {Email}", user.Email);
                return RedirectToPage("/Error");
            }

            _logger.LogInformation("✅ Email підтверджено для {Email}", user.Email);

            // 🔐 Автоматичний вхід
            await _signInManager.SignInAsync(user, isPersistent: false);

            // 🔁 Перенаправлення до профілю
            return RedirectToAction("Index", "Profile");
        }
    }
}
