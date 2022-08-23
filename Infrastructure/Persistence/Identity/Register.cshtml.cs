using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IEmailService _emailService;
        private readonly ReversiDbContext _reversiDbContext;
        private readonly IGoogleCaptchaService _service;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IEmailService emailService,
            ReversiDbContext reversiContext,
            IGoogleCaptchaService service)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _emailService = emailService;
            _reversiDbContext = reversiContext;
            _service = service;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(128, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 12)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string RecaptchaToken { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var captchaResult = await _service.VerifyCaptchaToken(Input.RecaptchaToken);

            if (!captchaResult)
                return Page();

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Something went wrong in the model: { ModelState.ErrorCount }");
                return Page();
            }

          
            var user = new IdentityUser { UserName = Input.UserName, Email = Input.Email };
            
            var result = await _userManager.CreateAsync(user, Input.Password);
            /*var modRoleResult = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Moderator"));
            var adminRoleResult = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Admin"));*/

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(err => err.Description);
                var errorMessage = string.Join(", ", errors);
                _logger.LogError($"Something went wrong with creating a user: {errorMessage}");
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            var spelerRoleResult = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Speler"));

            _logger.LogInformation($"Assigned 'speler' role to user: {user.Id}");
            _logger.LogInformation("User created a new account with password.");

            _reversiDbContext.Spelers.Add(new Speler(user.Id, user.UserName));
            var insertResult = await _reversiDbContext.SaveChangesAsync();

            _logger.LogInformation($"Inserted a new Speler entity with id: {user.Id} into the database");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            _emailService.Send("basschimmel@outlook.com", "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            
            


            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
