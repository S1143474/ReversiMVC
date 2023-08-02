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
using Newtonsoft.Json;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Speler")]
    public class SpelerController : ControllerBase
    {
        private readonly ReversiDbContext _context;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;


        public SpelerController(IHttpContextAccessor httpContextAccessor, ReversiDbContext context, ApplicationDbContext applicationContext, UserManager<IdentityUser> userManager) : base(httpContextAccessor)
        { 
            _context = context;
            _userManager = userManager;
            _applicationDbContext = applicationContext;
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
            var spelers = await _context.Spelers.ToListAsync();
            var roles = await _applicationDbContext.UserClaims.ToListAsync();

            var users = new List<AssignRolesUserDTO>();
            foreach (var user in spelers)
            {
                var roleUser = new AssignRolesUserDTO
                {
                    Guid = user.Guid,
                    Name = user.Naam,
                };

                var userRoles = new List<string>();
                foreach (var role in roles)
                {
                    if (role.UserId.Equals(user.Guid.ToString()))
                    {
                        userRoles.Add(role.ClaimValue);
                    }
                }

                roleUser.Roles = userRoles;
                users.Add(roleUser);
            }

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
            var userRoles = JsonConvert.DeserializeObject<List<AsignRolesDTO>>(list);
/*            var user = await _userManager.GetUserAsync(User);
*/
            foreach (var userRole in userRoles)
            {
/*                var claims = await _applicationDbContext.UserClaims.Where(x => x.UserId.Equals(userRole.id)).ToListAsync();
*/                /*var claimss = User.Claims.Where(x => x.Type.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")).ToList();*/
                var user = _userManager.Users.Where(x => x.Id == userRole.id).FirstOrDefault();
                var claimss = await _userManager.GetClaimsAsync(user);

                /*foreach(var claim in claims)
                {
                    if(!claim.ClaimValue.Equals(userRole.role))
                    {
                        _applicationDbContext.UserClaims.Add(new Microsoft.AspNetCore.Identity.IdentityUserClaim<string>
                        {
                            UserId = userRole.id,
                            ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                            ClaimValue = userRole.role,
                        });
                    }
                }*/
                /*var isAdmin = claims.Where(x => x.ClaimValue.Equals("Admin")).FirstOrDefault() != null;
                var isModerator = claims.Where(x => x.ClaimValue.Equals("Moderator")).FirstOrDefault() != null;
                var isSpeler = claims.Where(x => x.ClaimValue.Equals("Speler")).FirstOrDefault() != null;*/
                var adminClaim = claimss.Where(x => x.Value.Equals("Admin")).FirstOrDefault();
                var moderatorClaim = claimss.Where(x => x.Value.Equals("Moderator")).FirstOrDefault();
                var spelerClaim = claimss.Where(x => x.Value.Equals("Speler")).FirstOrDefault();

                var isAdmin = claimss.Where(x => x.Value.Equals("Admin")).FirstOrDefault() != null;
                var isModerator = claimss.Where(x => x.Value.Equals("Moderator")).FirstOrDefault() != null;
                var isSpeler = claimss.Where(x => x.Value.Equals("Speler")).FirstOrDefault() != null;

                if (isAdmin && userRole.role.Equals("Moderator") || userRole.role.Equals("Speler"))
                {
                    var oldClaim = _applicationDbContext.UserClaims.Where(x => x.UserId.Equals(userRole.id) && x.ClaimValue.Equals("Admin")).FirstOrDefault();

                    if (oldClaim != null)
                    {                        
                        await _userManager.RemoveClaimAsync(user, oldClaim.ToClaim());
/*                        _applicationDbContext.UserClaims.Remove(oldClaim);
*/                    }
                }

                if (isModerator && userRole.role.Equals("Speler"))
                {
                    var oldClaim = _applicationDbContext.UserClaims.Where(x => x.UserId.Equals(userRole.id) && x.ClaimValue.Equals("Moderator")).FirstOrDefault();

                    if (oldClaim != null)
                    {
                        await _userManager.RemoveClaimAsync(user, oldClaim.ToClaim());
/*                        _applicationDbContext.UserClaims.Remove(oldClaim);
*/                    }
                }

                if (isSpeler && !isAdmin && userRole.role.Equals("Admin"))
                {
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, userRole.role));

                    /*_applicationDbContext.UserClaims.Add(new Microsoft.AspNetCore.Identity.IdentityUserClaim<string>
                    {
                        UserId = userRole.id,
                        ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                        ClaimValue = userRole.role,
                    });*/

                    if (!isModerator)
                    {
                        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Moderator"));

                        /*_applicationDbContext.UserClaims.Add(new Microsoft.AspNetCore.Identity.IdentityUserClaim<string>
                        {
                            UserId = userRole.id,
                            ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                            ClaimValue = "Moderator",
                        });*/
                    }
                }

                if (isSpeler && !isModerator && userRole.role.Equals("Moderator")) 
                {
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, userRole.role));

                    /*_applicationDbContext.UserClaims.Add(new Microsoft.AspNetCore.Identity.IdentityUserClaim<string>
                    {
                        UserId = userRole.id,
                        ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                        ClaimValue = userRole.role,
                    });*/
                }
                await _userManager.UpdateAsync(user);
            }

            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(AsignRoles));
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Monitoring()
        {
            return View();
        }

        [HttpGet]
        [Route("[controller]/Monitoring/{id}")]
        public async Task<IActionResult> MonitoringPlayer()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/[action]")]
        /*[Authorize(Policy = "Moderator")]
        [Authorize(Policy = "Admin")]*/
        public async Task<IActionResult> MonitoringPost(string userId, string reason)
        {
            var result = await Mediator.Send(new DeleteSpelerCommand
            {
                UserIdToDelete = userId,
                Reason = reason
            });

            return RedirectToAction(nameof(Monitoring));
        }

        /*     // GET: Speler/Details/5
             public async Task<IActionResult> Details(string id)
             {
                 if (id == null)
                 {
                     return NotFound();
                 }

                 var spelerModel = await _context.Spelers
                     .FirstOrDefaultAsync(m => m.Guid == id);
                 if (spelerModel == null)
                 {
                     return NotFound();
                 }

                 return View(spelerModel);
             }
     */
        // GET: Speler/Create
        /*public async Task<IActionResult> Create(string ReturnUrl)
        {
            // TODO: Do something when result equals false.
            var result = await Mediator.Send(new CreateSpelerCommand
            {
                UserId = UserId,
                UserName = UserName
            });

            return LocalRedirect(ReturnUrl);
        }*/

        // POST: Speler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Guid,Naam,AantalGewonnen,AantalVerloren,AantalGelijk")] Speler spelerModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spelerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(spelerModel);
        }*/

        // GET: Speler/Edit/5
        /*public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spelerModel = await _context.Spelers.FindAsync(id);
            if (spelerModel == null)
            {
                return NotFound();
            }
           return View(spelerModel);
        }*/

        // POST: Speler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Guid,Naam,AantalGewonnen,AantalVerloren,AantalGelijk")] Speler spelerModel)
        {
            if (id != spelerModel.Guid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spelerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpelerModelExists(spelerModel.Guid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(spelerModel);
        }*/

        // GET: Speler/Delete/5
        /*public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spelerModel = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (spelerModel == null)
            {
                return NotFound();
            }

            return View(spelerModel);
        }*/

        // POST: Speler/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var spelerModel = await _context.Spelers.FindAsync(id);
            _context.Spelers.Remove(spelerModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        /*private bool SpelerModelExists(string id)
        {
            return _context.Spelers.Any(e => e.Guid == id);
        }*/
    }
}
