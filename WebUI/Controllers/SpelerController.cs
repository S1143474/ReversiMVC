using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Spelers.Commands.CreateSpeler;
using Application.Spelers.Commands.DeleteSpeler;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SpelerController : ControllerBase
    {
        private readonly ReversiDbContext _context;

        public SpelerController(IHttpContextAccessor httpContextAccessor, ReversiDbContext context) : base(httpContextAccessor)
        {
            _context = context;
        }

        // GET: Speler
        public async Task<IActionResult> Index()
        {
            return View(await _context.Spelers.ToListAsync());
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> AsignRoles()
        {
            return View();
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Monitoring()
        {
            return View();
        }

        [HttpGet]
        [Route("[controller]/Monitoring/{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> MonitoringPlayer()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/[action]")]
        [Authorize(Roles = "Admin,Moderator")]
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
