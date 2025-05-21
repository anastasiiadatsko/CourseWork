using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using WebApplication1.Models;
using WebApplication1.Services;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ForgotPasswordModel> _logger;

        public ForgotPasswordModel(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            ILogger<ForgotPasswordModel> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public ForgotPasswordViewModel Input { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                _logger.LogWarning("⚠️ Email не знайдено або не підтверджено: {Email}", Input.Email);
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
    "/Account/ResetPassword",
    null,
    new { area = "Identity", code, email = user.Email },
    Request.Scheme);


            await _emailSender.SendEmailAsync(
                Input.Email,
                "Скидання паролю",
                $"<h3>Скидання паролю</h3><p>Натисніть <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>тут</a>, щоб скинути пароль.</p>");

            _logger.LogInformation("📧 Email для скидання паролю надіслано: {Email}", Input.Email);

            return RedirectToPage("./ForgotPasswordConfirmation");
        }
    }
}
