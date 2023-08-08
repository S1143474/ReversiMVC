using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Hubs;
using Application.Spellen.Commands.FinishedSpel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Application.Spelers.Commands.DeleteSpeler
{
    public class DeleteSpelerCommand : IRequest<bool>
    {
        public Guid UserIdToDelete { get; set; }
        public string Reason { get; set; }
    }

    public class DeleteSpelerCommandHandle : IRequestHandler<DeleteSpelerCommand, bool>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IReversiDbContext _reversiDbContext;
        private readonly ISpelService _spelService;
        private readonly IHubContext<ReversiHub, IReversiHub> _hub;
        

        public DeleteSpelerCommandHandle(UserManager<IdentityUser> userManager, IReversiDbContext reversiDbContext, ISpelService spelService, IHubContext<ReversiHub, IReversiHub> hub)
        {
            _userManager = userManager;
            _reversiDbContext = reversiDbContext;
            _spelService = spelService;
            _hub = hub;
        }

        public async Task<bool> Handle(DeleteSpelerCommand request, CancellationToken cancellationToken)
        {
            var spel = await _spelService.RetrieveSpelOverSpelerToken(request.UserIdToDelete);

            // Check if spel is already being played
            if (spel.Speler2Token == null)
            {
                var deleteResult = _spelService.DeleteSpelUnFinished(spel.Token);
            } else
            {
                var surrenderSpelResult = await _spelService.SurrenderSpel(request.UserIdToDelete, spel.Token);

                if (surrenderSpelResult && spel.Token != null)
                {
                    var result = await _spelService.RetrieveFinishedSpelOverSpelerToken(request.UserIdToDelete);

                    if (request.UserIdToDelete == result.Speler1Token)
                    {
                        await _hub.Clients.User(result.Speler2Token.ToString()).Redirect($"spel/Result");
                    } else
                    {
                        await _hub.Clients.User(result.Speler1Token.ToString()).Redirect($"spel/Result");
                    }
                }
            }

            var speler = await _reversiDbContext.Spelers.FindAsync(request.UserIdToDelete);
            var removedSpeler = _reversiDbContext.Spelers.Remove(speler);

            await _reversiDbContext.SaveChangesAsync(cancellationToken);

            var identityUserToDelete = await _userManager.FindByIdAsync(request.UserIdToDelete.ToString());
            var identityResult = await _userManager.DeleteAsync(identityUserToDelete);

            await _hub.Clients.User(request.UserIdToDelete.ToString()).OnDeletedUser($"Your account has been deleted for the reason: {request.Reason}");
            return identityResult.Succeeded;
/*            return true;
*/        }
    }
}
