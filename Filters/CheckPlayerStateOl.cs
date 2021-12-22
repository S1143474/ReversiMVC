using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using ReversiMvcApp.Models.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ReversiMvcApp.SpelService;

namespace ReversiMvcApp.Annotations
{
    public class CheckPlayerStateOl : ActionFilterAttribute, IAsyncActionFilter
    {
        public const string SESSION_KEY_PLAYER_STATE = "PlayerState";
        
        public const int AVAILABLE = 0;
        public const int WAITING = 1;
        public const int PLAYING = 2;

        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    context.Result = new RedirectResult("/Spel/Details/5");
        //    base.OnActionExecuted(context);
        //    //context.HttpContext.Response.Clear();
        //}

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    context.Result = new RedirectResult("/Spel/Details");
        //    context.HttpContext.Response.Clear();
        //}

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.Result = new RedirectResult("/Spel/Index");
            await base.OnActionExecutionAsync(context, next);
            try
            {
                SpelService service = new SpelService();
                ISession session = context.HttpContext.Session;

                if (context.HttpContext.User.Identity.IsAuthenticated) // Check if authenticated
                {
                    string currentUserID = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    SpelJsonModel model = await GetSpel(service, currentUserID);

                    if (session.Keys.Contains(SESSION_KEY_PLAYER_STATE)) // Check if player state is in the session.
                    {
                        if (session.GetInt32(SESSION_KEY_PLAYER_STATE) == PLAYING) // Redirect player to details page when still participating a game.
                        {
                            context.HttpContext.Response.Redirect($"/Spel/Details/{model.Token}");

                        }
                        else if (session.GetInt32(SESSION_KEY_PLAYER_STATE) == WAITING) // Redirect player to Spel index when waiting for an aponent
                        {
                            //context.Result = new LocalRedirectResult("/Spel/Index");
                            context.HttpContext.Response.Redirect("/Spel/Index");
                        }
                    }
                    else // Session is empty
                    {
                        if (model != null && model.Speler1Token != null && model.Speler2Token != null) // Check if player is still playing a game and set session state.
                        {
                            session.SetInt32(SESSION_KEY_PLAYER_STATE, PLAYING);
                        }
                        else if (model != null && model.Speler1Token != null && model.Speler2Token == null) // Check if player is waiting for an oponent and set session state.
                        {
                            session.SetInt32(SESSION_KEY_PLAYER_STATE, WAITING);
                        }
                        else // if non of the above, player is still available for a new game.
                        {
                            session.SetInt32(SESSION_KEY_PLAYER_STATE, AVAILABLE);
                        }
                    }
                }
            }
            finally
            {
                //Controller controller = (Controller)context.Controller;
                //controller.Url.Action(new UrlActionContext() { Action = "Details", Controller = "Spel"});
                // context.Result = new RedirectToActionResult("Details", "Spel", );
                await base.OnActionExecutionAsync(context, next);
            }

            //context.HttpContext.Response.Clear();
        }

        //public async void OnActionEx(AuthorizationFilterContext context)
        //{

        //}

        private async Task<SpelJsonModel> GetSpel(SpelService service, string id) => await service.RetrieveSpelViaSpelerToken(id);

















        // Session Variables
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private ISession _session => AppContext.Current.Session;
        private ISpelService _service { get; set; }

        //public CheckPlayerState(IHttpContextAccessor httpContextAccessor) : base()
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}
        //public CheckPlayerState()
        //{
        //    //_httpContextAccessor = new GetContextClass()._httpContextAccessor;
        //    if (/*_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated &&*/ _session.Keys.Contains(SESSION_KEY_PLAYER_STATE)) // SessionKey does exist 
        //    {
        //        int state = (int)_session.GetInt32(SESSION_KEY_PLAYER_STATE);

        //        // Check if the Player state is whitin 0 and 2
        //        if (state >= AVAILABLE && state <= PLAYING)
        //        {
        //            //List<SpelJsonModel> modelList = await _service.ReturnListOfSpellen();
        //            //Task<SpelJsonModel> task = ;
        //            SpelJsonModel model = GetSpel().Result;

        //            if (state == PLAYING)
        //            {
        //                //_httpContextAccessor.HttpContext.Response.RedirectToAction();
        //                AppContext.Current.Response.Redirect($"/Spel/Details/{model.Token}");
        //            }
        //        }

        //    }
        //    else // SessionKey doesn't exist yet.
        //    {
        //        _session.SetInt32(SESSION_KEY_PLAYER_STATE, AVAILABLE);
        //    }
        //}

    }
}
