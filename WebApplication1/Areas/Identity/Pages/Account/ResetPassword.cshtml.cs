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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ResetPasswordModel> _logger;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager,
                                   SignInManager<ApplicationUser> signInManager,
                                   ILogger<ResetPasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public ResetPasswordViewModel Input { get; set; } = new();

        public IActionResult OnGet(string code, string email)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(email))
            {
                _logger.LogError("⛔ Код або email не передані");
                return RedirectToPage("./Login");
            }

            Input.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            Input.Email = email;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user == null)
            {
                _logger.LogWarning("⛔ Користувача не знайдено: {Email}", Input.Email);
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Token, Input.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("✅ Пароль оновлено для {Email}", user.Email);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}