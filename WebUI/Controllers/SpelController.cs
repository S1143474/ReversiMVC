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
using Application.Spellen.Queries.GetSpellen;
using AutoMapper;
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
    [Authorize(Roles = "Speler")]
    /*[Authorize(Roles = "Admin")]*/
    [ServiceFilter(typeof(StillPlayingFilter))]
    public class SpelController : ControllerBase
    {   
        private readonly ILogger<SpelController> _logger;
        private readonly IMapper _mapper;

        public SpelController(ILogger<SpelController> logger, IMapper mapper, IHttpContextAccessor accessor) : base(accessor)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> AvailableGames()
        {
            var result = await Mediator.Send(new GetAvailableSpellenListQuery());

            return View(result);
        }

        [Route("[controller]/[action]/{id}")]
        [HttpGet]
        public async Task<ActionResult> Reversi(Guid id)
        {
            var result = await Mediator.Send(new GetSpelQuery()
            {
                Id = id,
                UserId = UserId
            });

            if (result == null)
                return NotFound();

            var speler1Token = Guid.Empty;
            Guid.TryParse(result.Speler1Token, out speler1Token);

            if (speler1Token != id && result.Speler2Token == "")
            {
                var spelToken = Guid.Empty;
                Guid.TryParse(result.Token, out spelToken);

                await Mediator.Send(new StartSpelCommand
                {
                    Speler2Token = UserId,
                    SpelToken = spelToken
                });

                result = await Mediator.Send(new GetSpelQuery()
                {
                    Id = id,
                    UserId = UserId
                });
            }

            return View(result);
        }
        
        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<ActionResult> Waiting(Guid id)
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

            var spelInQueue = await Mediator.Send(command);

            // TODO: Create Toast message when spel is not created.
            if (spelInQueue != null)
                return RedirectToAction(nameof(Waiting), new { id = spelInQueue.Token });

            return View();
        }

        [HttpGet]
        [Route("Spel")]
        public async Task<ActionResult> Menu()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> History()
        {
            var result = await Mediator.Send(new GetSpellenHistoryQuery
            {
                SpelerToken = UserId
            });

            return View(result);
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
