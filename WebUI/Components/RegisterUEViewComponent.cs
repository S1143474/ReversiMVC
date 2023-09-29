using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Components
{
    public class RegisterUEViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
