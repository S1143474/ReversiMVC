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
            _logger.LogInformation($"User with id: {UserId} requested to retrieve all available games");
            var result = await Mediator.Send(new GetAvailableSpellenListQuery());
            _logger.LogInformation($"User with id: {UserId} retrieved {result.Count} amount of available games");

            return View(result);
        }

        [Route("[controller]/[action]/{id}")]
        [HttpGet]
        public async Task<ActionResult> Reversi(Guid id)
        {
            _logger.LogInformation($"User with id: {UserId} requested to retrieve a spel with id: {id}");
            var result = await Mediator.Send(new GetSpelQuery()
            {
                Id = id,
                UserId = UserId
            });

            if (result == null)
            {
                _logger.LogInformation($"User with id: {UserId} request for spel with id: {id} has not been found.");
                return NotFound();
            }

            _logger.LogInformation($"User with id: {UserId} request for spel with id: {id} is found.");
            var speler1Token = Guid.Empty;
            Guid.TryParse(result.Speler1Token, out speler1Token);

            if (speler1Token != id && result.Speler2Token == "")
            {
                var spelToken = Guid.Empty;
                Guid.TryParse(result.Token, out spelToken);

                _logger.LogInformation($"User with id: {UserId} request to start spel with id: {id}");
                await Mediator.Send(new StartSpelCommand
                {
                    Speler2Token = UserId,
                    SpelToken = spelToken
                });

                _logger.LogInformation($"User with id: {UserId} requested to retrieve spel with id: {id} after starting a match.");
                result = await Mediator.Send(new GetSpelQuery()
                {
                    Id = id,
                    UserId = UserId
                });
                _logger.LogInformation($"User with id: {UserId} request for spel with id: {id} is found and started");
            }

            return View(result);
        }
        
        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<ActionResult> Waiting(Guid id)
        {
            _logger.LogInformation($"User with id: {UserId} request for spel with id: {id}.");
            var result = await Mediator.Send(new StartSpelCommand()
            {
                Speler2Token = UserId,
                SpelToken = id
            });

            if (result == SpelState.Error)
            {
                _logger.LogInformation($"User with id: {UserId} request for spel with id: {id} has not been found something has gone wrong -> SpelState.Error");
                TempData["error"] = "error";
                return new RedirectToActionResult(nameof(AvailableGames), "Spel", new {});
            }

            if (result == SpelState.Playing)
            {
                _logger.LogInformation($"User with id: {UserId} request for spel with id: {id} has been found and is still in playing mode.");
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
            _logger.LogInformation($"User with id: {UserId} request to create new spel page.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string omschrijving)
        {
            _logger.LogInformation($"User with id: {UserId} request to create new spel with the following description: {omschrijving}.");
            CreateSpelCommand command = new CreateSpelCommand
            {
                Description = omschrijving,
                PlayerToken = UserId
            };

            var spelInQueue = await Mediator.Send(command);

            if (spelInQueue != null)
            {
                _logger.LogInformation($"User with id: {UserId} created a spel: {spelInQueue.Token}");
                return RedirectToAction(nameof(Waiting), new { id = spelInQueue.Token });
            }

            _logger.LogInformation($"User with id: {UserId} some thing(s) have gone wrong creating a spel.");
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
            _logger.LogInformation($"User with id: {UserId} requests to see spellen history.");
            var result = await Mediator.Send(new GetSpellenHistoryQuery
            {
                SpelerToken = UserId
            });

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Result()
        {
            _logger.LogInformation($"User with id: {UserId} has finished a spel requested for the result.");
            var result = await Mediator.Send(new GetSpelFinishedResultsQuery { SpelerToken = UserId });

            if (result == null)
            {
                _logger.LogInformation($"User with id: {UserId} requested to see a finished spel but nothing has been found.");
                return RedirectToAction(nameof(Menu));
            }

            _logger.LogInformation($"User with id: {UserId} requested to see a finished spel and found that he is the winner: {result.IsWinner}.");
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> GlobalStats()
        {
            _logger.LogInformation($"User with id: {UserId} has requested to see the global stats.");
            var result = await Mediator.Send(new GetSpelersQuery());

            return View(result);
        }
    }
}
