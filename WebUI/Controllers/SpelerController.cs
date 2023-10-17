using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Spelers.Commands.CreateSpeler;
using Application.Spelers.Commands.DeleteSpeler;
using Application.Spelers.Commands.RoleAsignment;
using Application.Spelers.Queries.GetRoles;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebUI.Controllers
{
    [Authorize]
    public class SpelerController : ControllerBase
    {
        private readonly ReversiDbContext _context;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<SpelerController> _logger;


        public SpelerController(
            IHttpContextAccessor httpContextAccessor,
            ReversiDbContext context,
            ApplicationDbContext applicationContext,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<SpelerController> logger
            ) : base(httpContextAccessor)
        { 
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationContext;
            _logger = logger;
        }

        // GET: Speler
        public async Task<IActionResult> Index()
        {
            return View(await _context.Spelers.ToListAsync());
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AsignRoles()
        {
            _logger.LogInformation($"User [Admin] with id: {UserId} requested the assign roles page.");
            var spelers = await _context.Spelers.ToListAsync();
            

            var users = new List<AssignRolesUserDTO>();
            foreach (var speler in spelers)
            {
                var user = _userManager.Users.Where(x => x.Id.Equals(speler.Guid.ToString())).FirstOrDefault();

                var roles = await _userManager.GetRolesAsync(user);
                var roleUser = new AssignRolesUserDTO
                {
                    Guid = speler.Guid,
                    Name = speler.Naam,
                };

               /* var userRoles = new List<string>();
                foreach (var role in roles)
                {
                    if (role.UserId.Equals(user.Guid.ToString()))
                    {
                        userRoles.Add(role.ClaimValue);
                    }
                }*/

                roleUser.Roles = roles.ToList();
                users.Add(roleUser);
            }

            _logger.LogInformation($"User [Admin] with id: {UserId} found {spelers.Count} Users.");
            var result = new AssignRolesDTO()
            {
                UserCount = spelers.Count,
                Users = users,
            };

            return View(result);
        }

        [HttpPost]
        [ActionName("AsignRoles")]
        [ValidateAntiForgeryToken]
        [Route("[controller]/[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AsignRolesPost(string list)
        {
            _logger.LogInformation($"User [Admin] with id: {UserId} requested to assign the following roles: {list}.");
            var userRoles = JsonConvert.DeserializeObject<List<AsignRolesDTO>>(list);

            foreach (var userRole in userRoles)
            {
                var user = _userManager.Users.Where(x => x.Id == userRole.id).FirstOrDefault();
                var roles = await _userManager.GetRolesAsync(user);

                var adminClaim = roles.Where(x => x.Equals("Admin")).FirstOrDefault();
                var moderatorClaim = roles.Where(x => x.Equals("Moderator")).FirstOrDefault();
                var spelerClaim = roles.Where(x => x.Equals("Speler")).FirstOrDefault();

                var isAdmin = roles.Where(x => x.Equals("Admin")).FirstOrDefault() != null;
                var isModerator = roles.Where(x => x.Equals("Moderator")).FirstOrDefault() != null;
                var isSpeler = roles.Where(x => x.Equals("Speler")).FirstOrDefault() != null;

                if (isAdmin && userRole.role.Equals("Moderator") || userRole.role.Equals("Speler"))
                {
                    if (adminClaim != null)
                    {                        
                       await _userManager.RemoveFromRoleAsync(user, adminClaim);
                    }
                }

                if (isModerator && userRole.role.Equals("Speler"))
                {
                    if (moderatorClaim != null)
                    {
                        await _userManager.RemoveFromRoleAsync(user, moderatorClaim);
                    }
                }

                if (isSpeler && !isAdmin && userRole.role.Equals("Admin"))
                {
                    await _userManager.AddToRoleAsync(user, userRole.role);

                    if (!isModerator)
                    {
                        await _userManager.AddToRoleAsync(user, "Moderator");
                    }
                }

                if (isSpeler && !isModerator && userRole.role.Equals("Moderator")) 
                {
                    await _userManager.AddToRoleAsync(user, userRole.role);
                }

                await _userManager.UpdateSecurityStampAsync(user);
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction(nameof(AsignRoles));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Monitoring()
        {
            _logger.LogInformation($"User [Moderator] with id: {UserId} requested to monitor.");

            var spelers = await _context.Spelers.ToListAsync();
            var users = new List<AssignRolesUserDTO>();
            _logger.LogInformation($"User [Moderator] with id: {UserId} retrieved {spelers.Count} spelers.");

            foreach (var speler in spelers)
            {
                var user = _userManager.Users.Where(x => x.Id == speler.Guid.ToString()).FirstOrDefault();

                var monitorUser = new AssignRolesUserDTO
                {
                    Guid = speler.Guid,
                    Name = speler.Naam,
                };

                users.Add(monitorUser);
            }

            var result = new AssignRolesDTO()
            {
                UserCount = spelers.Count,
                Users = users,
            };

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/[action]")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> MonitoringPost(Guid userId, string reason)
        {
            _logger.LogInformation($"User [Moderator] with id: {UserId} requested to delete speler: {userId} for the reaseon: {reason}");
            var result = await Mediator.Send(new DeleteSpelerCommand
            {
                UserIdToDelete = userId,
                Reason = reason
            });

            if (result)
            {
                _logger.LogInformation($"User [Moderator] with id: {UserId} sucessfully deleted user: {userId}");
            } else
            {
                _logger.LogInformation($"User [Moderator] with id: {UserId} wasn't able to delete user: {userId}");
            }

            return RedirectToAction(nameof(Monitoring));
        }

        [HttpGet]
        [Route("[controller]/Monitoring/{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> MonitoringPlayer()
        {
            return View();
        }
    }
}
