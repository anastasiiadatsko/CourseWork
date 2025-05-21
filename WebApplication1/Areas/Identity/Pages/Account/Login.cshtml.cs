using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task OnGetAsync(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = returnUrl ?? Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            if (!ModelState.IsValid)
                return Page();

            var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);

            if (user == null)
            {
                _logger.LogWarning("❌ Email не знайдено: {Email}", Input.Email);
                ModelState.AddModelError(string.Empty, "Невдала спроба входу.");
                return Page();
            }

            if (!await _signInManager.UserManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogWarning("❌ Email не підтверджено: {Email}", Input.Email);
                ModelState.AddModelError(string.Empty, "Підтвердіть email перед входом.");
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("✅ Вхід успішний: {Email}", Input.Email);
                return LocalRedirect(ReturnUrl);
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("🚫 Акаунт заблоковано: {Email}", Input.Email);
                return RedirectToPage("./Lockout");
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = ReturnUrl, Input.RememberMe });
            }

            _logger.LogWarning("❌ Невдала спроба входу: {Email}", Input.Email);
            ModelState.AddModelError(string.Empty, "Невдала спроба входу.");
            return Page();
        }
    }
}
