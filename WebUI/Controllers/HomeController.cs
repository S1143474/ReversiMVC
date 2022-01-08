using System.Diagnostics;
using System.Threading.Tasks;
using Application.Spelers.Queries.GetSpeler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebUI.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor accessor) : base(accessor)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var query = new GetSpelerByIdQuery()
            {
                UserId = UserId,
                Naam = UserName
            };

            var result = await Mediator.Send(query);

            /*if (IsMobileDevice)
            {
                return RedirectToPage("/Account/MobileChooseAction", new { area = "Identity" });
            }
            return RedirectToPage("/Account/MobileChooseAction", new { area = "Identity" });*/

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
