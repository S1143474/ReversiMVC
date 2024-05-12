using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebUI.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly ILogger<IndexModel> _logger;
        private readonly IGoogleCaptchaService _captchaService;
        private readonly IEmailService _emailService;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<IndexModel> logger,
            IGoogleCaptchaService captchaService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _captchaService = captchaService;
            _emailService = emailService;
        }

        public string Username { get; set; }

        [BindProperty, TempData]
        public string StatusMessage { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            public string RecaptchaToken { get; set; }

        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError($"Unable to load user with ID: {_userManager.GetUserId(User)}");
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var captchaResult = await _captchaService.VerifyCaptchaToken(Input.RecaptchaToken);
            
            if (!captchaResult)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError($"Unable to load user with ID {_userManager.GetUserId(User)}.");
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Modelstate for changing phone is invalid");
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            _logger.LogInformation($"User with id: {user.Id} retrieved its phone number.");

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    _logger.LogError($"Something went wrong when trying to set a new phone number for user with id: {user.Id}");
                    foreach (var err in setPhoneResult.Errors)
                    {
                        _logger.LogError($"{err}");
                    }

                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return Page();
                }

                _logger.LogInformation($"Changed phone number for user with id: {user.Id}");
            }

            await _signInManager.RefreshSignInAsync(user);
            /*var emailAdress = await _userManager.GetEmailAsync(user);
            if (emailAdress is not null)
            {
                var callbackUrl = Url.Page(
                    "/Account/Manage/Index",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id },
                    protocol: Request.Scheme);

                var callbackUrlUpdatePass = Url.Page(
                    "/Account/Manage/ChangePassword",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id },
                    protocol: Request.Scheme);

                _emailService.Send("basschimmel@outlook.com", "Phone number updated",
                    $"Hi {user.UserName},<br/><br/> Your phone number has been updated.<br/> If this wasn't you consider <a href='{HtmlEncoder.Default.Encode(callbackUrlUpdatePass)}'>changing your password</a> as it may be breached and try to <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>update your phone number</a>.");
            }*/

            StatusMessage = "Your profile has been updated";
            return Page();
        }
    }
}
