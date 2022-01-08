using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Identity.Pages.Account.Manage
{
    [AllowAnonymous]
    public class MobileChooseAction
    {
        public async Task OnGetAsync(string returnUrl = null)
        {

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            return null;
        }
    }
}
