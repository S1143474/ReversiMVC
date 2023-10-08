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
    [Authorize]
    public class SpelerController : ControllerBase
    {
        private readonly ReversiDbContext _context;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;


        public SpelerController(IHttpContextAccessor httpContextAccessor, ReversiDbContext context, ApplicationDbContext applicationContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : base(httpContextAccessor)
        { 
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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
/*                await _signInManager.RefreshSignInAsync(user);
*/            }
            return RedirectToAction(nameof(AsignRoles));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Monitoring()
        {
            var spelers = await _context.Spelers.ToListAsync();
            var users = new List<AssignRolesUserDTO>();

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
            var result = await Mediator.Send(new DeleteSpelerCommand
            {
                UserIdToDelete = userId,
                Reason = reason
            });

            return RedirectToAction(nameof(Monitoring));
        }

        [HttpGet]
        [Route("[controller]/Monitoring/{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> MonitoringPlayer()
        {

            return View();
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
