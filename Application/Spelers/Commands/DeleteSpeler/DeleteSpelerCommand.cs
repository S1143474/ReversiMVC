using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Spelers.Commands.DeleteSpeler
{
    public class DeleteSpelerCommand : IRequest<bool>
    {
        public string UserIdToDelete { get; set; }
        public string Reason { get; set; }
    }

    public class DeleteSpelerCommandHandle : IRequestHandler<DeleteSpelerCommand, bool>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IReversiDbContext _reversiDbContext;
        
        public DeleteSpelerCommandHandle(UserManager<IdentityUser> userManager, IReversiDbContext reversiDbContext)
        {
            _userManager = userManager;
            _reversiDbContext = reversiDbContext;
        }

        public async Task<bool> Handle(DeleteSpelerCommand request, CancellationToken cancellationToken)
        {
            var speler = await _reversiDbContext.Spelers.FindAsync(request.UserIdToDelete);
            var removedSpeler = _reversiDbContext.Spelers.Remove(speler);

            await _reversiDbContext.SaveChangesAsync(cancellationToken);

            var identityUserToDelete = await _userManager.FindByIdAsync(request.UserIdToDelete);
            var identityResult = await _userManager.DeleteAsync(identityUserToDelete);
            return identityResult.Succeeded;
        }
    }
}
