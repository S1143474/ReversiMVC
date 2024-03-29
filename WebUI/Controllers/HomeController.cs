﻿using System.Diagnostics;
using System.Threading.Tasks;
using Application.Spelers.Queries.GetSpeler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using WebUI.Middleware.ExceptionMiddleWare;

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

        [HttpGet("{err}")]
        public async Task<IActionResult> Index(string? err)
        {
            if (err is "reqerror")
            {
                _logger.LogInformation($"User with id: {UserId} tried to connect with the Reversi API but the API wouldn't react.");
                ViewData["errormessage"] =
                    "Unable to play reversi because the target computer has actively refused the connection. Please try again later.";
            }

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
