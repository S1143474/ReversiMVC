using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReversiMvcApp.Annotations;
using ReversiMvcApp.Models.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using ReversiMvcApp.Helpers;
using System.Threading.Tasks;
using ReversiMvcApp.Models.Session;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using ReversiMvcApp.Filters;
using ReversiMvcApp.Hubs;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;

namespace ReversiMvcApp.Controllers
{

    [ServiceFilter(typeof(StillPlayingFilter))]
    public class SpelController : Controller
    {
        // API Service
        private readonly ISpelService _service;
        
        private readonly ILogger<SpelController> _logger;
        private readonly IHubContext<ReversiHub> _reversiHub;
        private readonly ReversiDbContext _context;

        public SpelController(ISpelService service, ILogger<SpelController> logger, IHubContext<ReversiHub> hubContext, ReversiDbContext context)
        {
            _service = service;
            _logger = logger;
            _reversiHub = hubContext;
            _context = context;
        }

        // GET: SpelController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<SpelJsonModel> modelList = await _service.ReturnListOfSpellen();

            if (HttpContext.Session.Keys.Contains(PlayerStateFilter.SESSION_KEY_PLAYER_STATE))
            {
                var result = HttpContext.Session.GetObjectFromBytes(PlayerStateFilter.SESSION_KEY_PLAYER_STATE);
            }
            return View(modelList);
        }

        // GET: SpelController/Details/5
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Details(string id)
        {
            SpelJsonModel model = await _service.RetrieveSpelViaToken(id);
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            SpelerModel player1 = _context.Spelers.Find((currentUserId == model.Speler1Token) ? model.Speler1Token : model.Speler2Token);
            SpelerModel player2 = _context.Spelers.Find((currentUserId == model.Speler1Token) ? model.Speler2Token : model.Speler1Token);

            if (player1 != null)
                ViewBag.Player1 = player1.Naam;

            if (player2 != null)
                ViewBag.Player2 = player2.Naam;

/*            await _reversiHub.Clients.User(model.Speler1Token).SendAsync("test", "new signalr message");
*/

            if (model.Speler2Token == null && model.Speler1Token != currentUserId)
            {
                //HttpContext.Session.SetObjectAsBytes(PlayerStateFilter.SESSION_KEY_PLAYER_STATE, new StateModel(currentUserId, PlayerStateFilter.PLAYING));
                await _service.JoinGameReversi(model.Token, currentUserId);
                await _reversiHub.Clients.User(model.Speler1Token).SendAsync("Start", model.Token, "Game has begun!");
            }

            return View(model);
        }

        // GET: SpelController/Create
        [HttpGet]
        [Authorize]
/*        [PlayerStateFilter]
*/        public ActionResult Create()
        {
            return View();
        }

        // POST: SpelController/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string omschrijving)
        {
            PlaceGameJsonObj placeGameJsonObj = new PlaceGameJsonObj()
            {
                PlayerToken = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Description = omschrijving
            };

            try
            {
                await _service.CreateSpel(placeGameJsonObj);

                HttpContext.Session.SetObjectAsBytes(PlayerStateFilter.SESSION_KEY_PLAYER_STATE, new StateModel(placeGameJsonObj.PlayerToken, PlayerStateFilter.WAITING));
                _reversiHub.Clients.User(placeGameJsonObj.PlayerToken);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        [Authorize]
/*        [PlayerStateFilter]
*/        public async Task<ActionResult> Options()
        {
            string currentUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            SpelJsonModel model = await _service.RetrieveSpelViaSpelerToken(currentUserID);

            if (model != null)
            {
                _logger.LogInformation($"Player: {currentUserID} is already playing a game: {model.Omschrijving} | {model.Token}.");
                return RedirectToAction(nameof(Details), new { id = model.Token});
            }
            
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult History()
        {
            return View();
        }

        // GET: SpelController/Edit/5
        //[HttpGet]
        //public async Task<ActionResult> Edit(string id)
        //{

        //    return View();
        //}

        //// POST: SpelController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: SpelController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: SpelController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
