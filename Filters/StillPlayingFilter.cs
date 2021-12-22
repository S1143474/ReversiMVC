using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReversiMvcApp.Controllers;
using ReversiMvcApp.Models.Json;


namespace ReversiMvcApp.Filters
{
    /*    public class StillPlayingFilter : ActionFilterAttribute
        {
            private ISpelService _spelService;

            private SpelJsonModel _spel;

            public StillPlayingFilter([FromServices] ISpelService service)
            {
                _spelService = service;
            }

            public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
            {
                var claim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                if (claim != null)
                {
                    _spel = await _spelService.RetrieveSpelViaSpelerToken(claim.Value);

                    if (_spel != null)
                    {
                        var routeData = context.RouteData.Values;

                        if (routeData.TryGetValue("controller", out var controller) &&
                            routeData.TryGetValue("action", out var action))
                        {

                            if (controller.Equals("Spel") && action.Equals("Details"))
                            {
                                await next();
                                return;
                            }

                            context.Result = new RedirectResult($"/Spel/Details/{_spel.Token}");

                        }

                    }
                }

                await next();
            }
        }*/
    public class StillPlayingFilter : ResultFilterAttribute
    {
        private ISpelService _spelService;

        private SpelJsonModel _spel;

        public StillPlayingFilter([FromServices]ISpelService service)
        {
            _spelService = service;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var claim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                _spel = await _spelService.RetrieveSpelViaSpelerToken(claim.Value);

                if (_spel != null)
                {
                    var routeData = context.RouteData.Values;

                    if (routeData.TryGetValue("controller", out var controller) &&
                        routeData.TryGetValue("action", out var action))
                    {

                        if (controller.Equals("Spel") && action.Equals("Details"))
                        {
                            await next();
                            return;
                        }

                        context.Result = new RedirectResult($"/Spel/Details/{_spel.Token}");

                    }

                }
            }

            await next();
        }
    }
}
