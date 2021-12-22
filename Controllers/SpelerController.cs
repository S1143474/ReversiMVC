﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;

namespace ReversiMvcApp.Controllers
{
    public class SpelerController : Controller
    {
        private readonly ReversiDbContext _context;

        public SpelerController(ReversiDbContext context)
        {
            _context = context;
        }

        // GET: Speler
        public async Task<IActionResult> Index()
        {
            return View(await _context.Spelers.ToListAsync());
        }

        // GET: Speler/Details/5
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

        // GET: Speler/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Speler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Guid,Naam,AantalGewonnen,AantalVerloren,AantalGelijk")] SpelerModel spelerModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spelerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(spelerModel);
        }

        // GET: Speler/Edit/5
        public async Task<IActionResult> Edit(string id)
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
        }

        // POST: Speler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Guid,Naam,AantalGewonnen,AantalVerloren,AantalGelijk")] SpelerModel spelerModel)
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
        }

        // GET: Speler/Delete/5
        public async Task<IActionResult> Delete(string id)
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

        // POST: Speler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var spelerModel = await _context.Spelers.FindAsync(id);
            _context.Spelers.Remove(spelerModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpelerModelExists(string id)
        {
            return _context.Spelers.Any(e => e.Guid == id);
        }
    }
}
