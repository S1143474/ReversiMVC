using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;
using ReversiMvcApp.Models.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using ReversiMvcApp.Annotations;

namespace ReversiMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ReversiDbContext _context;

        // Api Service
        private readonly ISpelService _service;

        public HomeController(ReversiDbContext context, ILogger<HomeController> logger, ISpelService service, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            ClaimsPrincipal currentUser = this.User;

            //HttpContext.Session.SetInt32(PlayerStateFilter.SESSION_KEY_PLAYER_STATE, -1);

            // Check if user is authenticated.
            // TODO: Make an filter for this.
            if (currentUser.Identity.IsAuthenticated)
            {
                string currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                SpelerModel model = _context.Spelers.FirstOrDefault(s => s.Guid.Equals(currentUserID));

                // Check if a speler objects doesn't exist in the reversiDb and create one.
                if (model == null)
                {
                    model = new SpelerModel(currentUserID, currentUser.Identity.Name);

                    _context.Spelers.Add(model);
                    _context.SaveChanges();
                }

                ViewBag.Wins = model.AantalGewonnen;
                ViewBag.Losses = model.AantalVerloren;
                ViewBag.Draws = model.AantalGelijk;
                ViewBag.FichesFlipped = model.AantalFicheFliped;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
