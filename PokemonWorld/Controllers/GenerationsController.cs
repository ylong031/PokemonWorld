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
    public class GenerationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GenerationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Generations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Generations.ToListAsync());
        }

        // GET: Generations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var generation = await _context.Generations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (generation == null)
            {
                return NotFound();
            }

            return View(generation);
        }

        // GET: Generations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Generations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Generation generation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(generation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(generation);
        }

        // GET: Generations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var generation = await _context.Generations.FindAsync(id);
            if (generation == null)
            {
                return NotFound();
            }
            return View(generation);
        }

        // POST: Generations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Generation generation)
        {
            if (id != generation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(generation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenerationExists(generation.Id))
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
            return View(generation);
        }

        // GET: Generations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var generation = await _context.Generations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (generation == null)
            {
                return NotFound();
            }

            return View(generation);
        }

        // POST: Generations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var generation = await _context.Generations.FindAsync(id);
            if (generation != null)
            {
                _context.Generations.Remove(generation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenerationExists(int id)
        {
            return _context.Generations.Any(e => e.Id == id);
        }
    }
}
