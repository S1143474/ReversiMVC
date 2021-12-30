using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Spellen.Commands.CreateSpel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Filters;

namespace WebUI.Controllers
{
    [ServiceFilter(typeof(StillPlayingFilter))]
    [Route("[controller]/{action=AvailableGames}")]
    public class SpelController : ControllerBase
    {   
        private readonly ILogger<SpelController> _logger;

        public SpelController(ILogger<SpelController> logger, IHttpContextAccessor accessor) : base(accessor)
        {
            _logger = logger;
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
        public async Task<ActionResult> Create(string omschrijving)
        {
            CreateSpelCommand command = new CreateSpelCommand
            {
                Description = omschrijving,
                PlayerToken = UserId
            };

            if (await Mediator.Send(command))
            {
                return RedirectToAction(nameof(AvailableGames));
            }

            return View();
        }

        [HttpGet]
        [Authorize]
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
