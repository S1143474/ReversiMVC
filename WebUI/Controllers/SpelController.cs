using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Spelers.Queries.GetSpelers;
using Application.Spelers.Queries.GetSpellen;
using Application.Spellen.Commands.CreateSpel;
using Application.Spellen.Commands.StartSpel;
using Application.Spellen.Queries.GetSpel;
using Application.Spellen.Queries.GetSpelFinishedResults;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using WebUI.Filters;

namespace WebUI.Controllers
{
    [Authorize]
    /*[Authorize(Roles = "Admin")]*/
    [ServiceFilter(typeof(StillPlayingFilter))]
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
            var result = await Mediator.Send(new GetAvailableSpellenListQuery());

            return View(result);
        }

        [Route("[controller]/[action]/{id}")]
        [HttpGet]
        public async Task<ActionResult> Reversi(string id)
        {
            var result = await Mediator.Send(new GetSpelQuery()
            {
                Id = id,
                UserId = UserId
            });

            if (result == null)
                return NotFound();

            return View(result);
        }
        
        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<ActionResult> Waiting(string id)
        {
            var result = await Mediator.Send(new StartSpelCommand()
            {
                Speler2Token = UserId,
                SpelToken = id
            });

            if (result == SpelState.Error)
            {
                TempData["error"] = "error";
                return new RedirectToActionResult(nameof(AvailableGames), "Spel", new {});
            }

            if (result == SpelState.Playing)
            {
                return new RedirectToActionResult(nameof(Reversi), "Spel", new
                {
                    id
                });
            }

            return View();
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

            var isSpelCreated = await Mediator.Send(command);

            // TODO: Create Toast message when spel is not created.
            if (isSpelCreated)
                return RedirectToAction(nameof(Waiting));

            return View();
        }

        [HttpGet]
        [Route("Spel")]
        public async Task<ActionResult> Menu()
        {
            return View();
        }

        [HttpGet]
        public ActionResult History()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Result()
        {
            var result = await Mediator.Send(new GetSpelFinishedResultsQuery { SpelerToken = UserId });

            if (result == null)
                return RedirectToAction(nameof(Menu));

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> GlobalStats()
        {
            var result = await Mediator.Send(new GetSpelersQuery());

            return View(result);
        }
    }
}
