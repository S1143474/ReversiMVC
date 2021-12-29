using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebUI.Controllers
{
    [Route("[controller]/{action=AvailableGames}")]
    public class SpelController : Controller
    {   
        private readonly ILogger<SpelController> _logger;

        public SpelController(ILogger<SpelController> logger)
        {
            logger = _logger;
        }

        [HttpGet]
        public async Task<ActionResult> AvailableGames()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Reversi(string id)
        {
            return View(null);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string description)
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Menu()
        {
            return View();
        }

        [HttpGet]
        public ActionResult History()
        {
            return View();
        }
    }
}
