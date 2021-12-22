using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReversiMvcApp.Models.Json;
using ReversiMvcApp.Models.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ReversiMvcApp.Helpers;
using Microsoft.AspNetCore.Routing;

namespace ReversiMvcApp.Annotations
{
    public class PlayerStateFilter : ActionFilterAttribute, IAsyncActionFilter, IActionFilter
    {

        public const string SESSION_KEY_PLAYER_STATE = "PlayerState";

        public const int AVAILABLE = 0;
        public const int WAITING = 1;
        public const int PLAYING = 2;

        private Controller _controller { get; set; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Execute before the action executes.
            SpelService service = new SpelService();
            ISession session = context.HttpContext.Session;
/*            _controller = (Controller)context.Controller;
*/
            if (context.HttpContext.User.Identity.IsAuthenticated) // Check if authenticated
            {
                string currentUserID = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                SpelJsonModel model = await GetSpel(service, currentUserID);

                if (session.Keys.Contains(SESSION_KEY_PLAYER_STATE)) // Check if player state is in the session.
                {
                    // Redirects player based on current state
                    RedirectPlayerBaseOnState(context, session, model);
                }
                else // Session is empty
                {
                    // Sets session if it doesn't exist
                    SetSessionPlayerState(session, model, currentUserID);

                    // Redirects player based on current state
                    RedirectPlayerBaseOnState(context, session, model);
                }
            } else
            {
                context.Result = new RedirectResult("/Spel/Index");
            }
            
            await base.OnActionExecutionAsync(context, next);
        }

        /// <summary>
        /// Sets the session of playerstate
        /// </summary>
        /// <param name="session"></param>
        /// <param name="model"></param>
        /// <param name="guid"></param>
        private void SetSessionPlayerState(ISession session, SpelJsonModel model, string guid)
        { 
            if (model != null && model.Speler1Token != null && model.Speler2Token != null) // Check if player is still playing a game and set session state.
            {
                session.SetObjectAsBytes(SESSION_KEY_PLAYER_STATE, new StateModel(guid, PLAYING));
            }
            else if (model != null && model.Speler1Token != null && model.Speler2Token == null) // Check if player is waiting for an oponent and set session state.
            {
                session.SetObjectAsBytes(SESSION_KEY_PLAYER_STATE, new StateModel(guid, WAITING));
            }
            else // if non of the above, player is still available for a new game.
            {
                session.SetObjectAsBytes(SESSION_KEY_PLAYER_STATE, new StateModel(guid, AVAILABLE));
            }
        }

        /// <summary>
        /// Method for redirecting and informing the user about their current state
        /// </summary>
        /// <param name="context"></param>
        /// <param name="session"></param>
        /// <param name="model"></param>
        private void RedirectPlayerBaseOnState(ActionExecutingContext context, ISession session2, SpelJsonModel model)
        {
            //StateModel stateModel = session.GetObjectFromBytes(SESSION_KEY_PLAYER_STATE);
            ISession session = context.HttpContext.Session;
            StateModel stateModel = session.GetObjectFromBytes(SESSION_KEY_PLAYER_STATE);

            string guid = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (model == null)
                return;
            // Check if the guids are equal
            if (stateModel.Guid.Equals(guid))
            {
                if (stateModel.State == PLAYING) // If player is already playing a game
                { 
                    // Redirect to that game he/she is playing
                    context.Result = new RedirectResult($"/Spel/Details/{model.Token}");

                    // Send message to the user that he/she is playing a game
                    SendStateMessage(PLAYING);
                }
                else if (stateModel.State == WAITING) // If player is waiting for an opponent
                {
                    // Redirect player to the index page.
                    context.Result = new RedirectResult("/Spel/Index");

                    // Send message to the user that he/she is already in a game.
                    SendStateMessage(WAITING);
                }
                else
                {
                    SendStateMessage(AVAILABLE);
                }
            } else // if guid were not equal
            {
                // Set the session to the from the new user
                SetSessionPlayerState(session, model, guid);
                // Redirect the user to the correct page.
                RedirectPlayerBaseOnState(context, session, model);
            }
        }

        /// <summary>
        /// Method sends the currentstate form the player to TempData
        /// </summary>
        /// <param name="context"></param>
        /// <param name="state"></param>
        private void SendStateMessage(int state)
        {
            _controller.TempData[SESSION_KEY_PLAYER_STATE] = state;
        }

        /// <summary>
        /// Task retrieves SpelJsonModel for checking if a player is already in a game.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<SpelJsonModel> GetSpel(SpelService service, string id) => await service.RetrieveSpelViaSpelerToken(id);

    }
}
