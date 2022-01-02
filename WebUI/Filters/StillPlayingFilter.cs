using System.Security.Claims;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebUI.Filters
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
        
        public StillPlayingFilter([FromServices]ISpelService service)
        {
            _spelService = service;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var claim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
             
            if (claim != null)
            {
                var spel = await _spelService.RetrieveSpelOverSpelerToken(claim.Value);

                if (spel != null)
                {
                    var routeData = context.RouteData.Values;

                    if (routeData.TryGetValue("controller", out var controller) &&
                        routeData.TryGetValue("action", out var action))
                    {

                        if (controller.Equals("Spel") && action.Equals("Waiting"))
                        {
                            await next();
                            return;
                        }

                        if (IsSpeler1Waiting(spel.speler2Token))
                        {
                            context.Result = new RedirectResult($"spel/Waiting/{spel.token}");
                            await next();
                            return;
                        }
                        /*context.HttpContext.Response.Redirect($"/Spel/Reversi/{_spel.token}");*/
                        context.Result = new RedirectResult($"/Spel/Reversi/{spel.token}");

                    }
                      
                }
            }

            await next();
        }

        /// <summary>
        /// Check if Speler 1 is waiting on a Speler 1
        /// </summary>
        /// <param name="speler2Token"></param>
        /// <returns></returns>
        private bool IsSpeler1Waiting(string speler2Token) => speler2Token is null;
    }
}
