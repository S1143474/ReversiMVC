using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using static WebUI.Filters.FilterHelpers;

namespace WebUI.Filters
{
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
                var spel = await _spelService.RetrieveSpelOverSpelerToken(Guid.Parse(claim.Value));

                if (spel != null)
                {
                    var routeData = context.RouteData.Values;

                    if (routeData.TryGetValue("controller", out var controller) &&
                        routeData.TryGetValue("action", out var action))
                    {

                        if (IsInWaitingView(controller, action) || IsInReversiView(controller, action) || IsInResultView(controller, action))
                        {
                            await next();
                            return;
                        }

                        if (IsSpeler1Waiting(spel.Speler2Token))
                        {
                            context.Result = new RedirectToActionResult("Waiting", "Spel", new { id = spel.Token });
                            await next();
                            return;
                        }

                        context.Result = new RedirectToActionResult("Reversi", "Spel", new { id = spel.Token });
                    }
                      
                }
            }

            await next();
        }
    }
}
