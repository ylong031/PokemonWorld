using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PokemonWorld.Data;

namespace PokemonWorld.Controllers
{
    public class PokemonStatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PokemonStatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PokemonStats
        public async Task<IActionResult> Index()
        {
            return View(await _context.PokemonStats.ToListAsync());
        }

        // GET: PokemonStats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pokemonStat = await _context.PokemonStats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pokemonStat == null)
            {
                return NotFound();
            }

            return View(pokemonStat);
        }

        // GET: PokemonStats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PokemonStats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] PokemonStat pokemonStat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pokemonStat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pokemonStat);
        }

        // GET: PokemonStats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pokemonStat = await _context.PokemonStats.FindAsync(id);
            if (pokemonStat == null)
            {
                return NotFound();
            }
            return View(pokemonStat);
        }

        // POST: PokemonStats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] PokemonStat pokemonStat)
        {
            if (id != pokemonStat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pokemonStat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PokemonStatExists(pokemonStat.Id))
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
            return View(pokemonStat);
        }

        // GET: PokemonStats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pokemonStat = await _context.PokemonStats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pokemonStat == null)
            {
                return NotFound();
            }

            return View(pokemonStat);
        }

        // POST: PokemonStats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pokemonStat = await _context.PokemonStats.FindAsync(id);
            if (pokemonStat != null)
            {
                _context.PokemonStats.Remove(pokemonStat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PokemonStatExists(int id)
        {
            return _context.PokemonStats.Any(e => e.Id == id);
        }
    }
}
