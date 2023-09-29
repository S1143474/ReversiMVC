using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Polly;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ICurrentUserService _currentUser;
        private readonly IGoogleCaptchaService _service;

        public LoginModel(SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            ICurrentUserService currentUserService,
            IGoogleCaptchaService service)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _currentUser = currentUserService;
            _service = service;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            /*[Required]
            [EmailAddress]
            public string Email { get; set; }*/
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            [Required]
            public string RecaptchaToken { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        private async Task<ClaimsIdentity> CreateIdentity(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var identity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                }, CookieAuthenticationDefaults.AuthenticationScheme);

            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return identity;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            var captchaResult = await _service.VerifyCaptchaToken(Input.RecaptchaToken);

            if (!captchaResult)
                return Page();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
/*                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);
*/              
                var user = await _userManager.FindByNameAsync(Input.UserName);

                if (user == null)
                {
                    _logger.LogInformation($"Username not found: {Input.UserName}", user);
                    ErrorMessage = "Username or password is incorrect.";
                    return Page();
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, lockoutOnFailure: true);

                ClaimsIdentity identity = await CreateIdentity(user);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

               
                if (result.Succeeded)
                {
                    await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, principal.Claims);
                    _logger.LogInformation($"User: {_currentUser.UserId} logged in.");
                   /* return RedirectToAction("Create", "Speler", new
                    {
                        ReturnUrl = returnUrl
                    });*/

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    _logger.LogInformation($"User: {_currentUser.UserId} needs to login with 2fa.");

                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning($"User: {_currentUser.UserId} account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    _logger.LogWarning($"User tried to login, but something went wrong");

                    ErrorMessage = "Username or password is incorrect.";
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
