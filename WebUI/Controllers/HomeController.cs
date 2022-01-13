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
            var result = await Mediator.Send(new GetSpelerByIdQuery
            {
                UserId = UserId,
                Naam = UserName
            });
            
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
