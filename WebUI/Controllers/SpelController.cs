using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Spelers.Queries.GetSpellen;
using Application.Spellen.Commands.CreateSpel;
using Application.Spellen.Commands.StartSpel;
using Application.Spellen.Queries.GetSpel;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Filters;

namespace WebUI.Controllers
{
    [Authorize]
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

        [HttpGet]
        [Route("[controller]/[action]/{id}")]
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

            if (result == SpelState.Playing)
            {
                return RedirectToAction(nameof(Reversi), new
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

            if (await Mediator.Send(command))
            {
                return RedirectToAction(nameof(AvailableGames));
            }

            return View();
        }

        [HttpGet]
        [Route("spel")]
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
