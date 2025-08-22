using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PokemonWorld.Data;
using PokemonWorld.Models;
using System;
using System.Security.Policy;
using System.Threading;

public class PokemonsController(ApplicationDbContext _context) : Controller
{



    /*  public async Task<IActionResult> Index(int[]? SelectedTypeIds)
      {
         var query = _context.Pokemons
              .Include(p => p.Generation)
              .Include(p => p.Types)
              .Include(p => p.StatValues)
                  .ThenInclude(sv => sv.PokemonStat)
              .AsQueryable();

          // Filter if any types are selected
          if (SelectedTypeIds != null && SelectedTypeIds.Length > 0)
          {
              query = query.Where(p => p.Types.Any(t => SelectedTypeIds.Contains(t.Id)));
          }

          var pokemons = await query.ToListAsync();

          ViewBag.Types = await _context.PokemonTypes.ToListAsync();





          return View(pokemons);


      }*/

    public async Task<IActionResult> Index(int[]? SelectedTypeIds)
    {
        var query = _context.Pokemons
            .Include(p => p.Generation)
            .Include(p => p.Types)
            .Include(p => p.StatValues)
                .ThenInclude(sv => sv.PokemonStat)
            .AsQueryable();

        // Filter if any types are selected
        if (SelectedTypeIds != null && SelectedTypeIds.Length > 0)
        {
            query = query.Where(p => p.Types.Any(t => SelectedTypeIds.Contains(t.Id)));
        }

        var pokemons = await query.ToListAsync();
        var types = await _context.PokemonTypes.ToListAsync();

        // Map to ViewModels (simplified version, you can expand later)
        var viewModel = new PokemonIndexViewModel
        {
            Pokemons = pokemons.Select(p => new PokemonViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Index = p.Index,
                IsLegendary=p.IsLegendary,
                GenerationName = p.Generation.Name,
                TypeNames = p.Types.Select(t => t.Name).ToList(),
                Stats = p.StatValues.Select(sv => new StatValueViewModel
                {
                    StatName = sv.PokemonStat.Name,
                    Value = sv.Value
                }).ToList()
            }).ToList(),

            Types = types.Select(t => new PokemonTypeViewModel
            {
                Id = t.Id,
                Name = t.Name
            }).ToList(),

            SelectedTypeIds = SelectedTypeIds
        };

        return View(viewModel);
    }





    // GET: Create
    public async Task<IActionResult> Create()
    {
        
        ViewBag.GenerationsList = new SelectList(await _context.Generations.ToListAsync(), "Id", "Name");
        ViewBag.Stats = await _context.PokemonStats.ToListAsync();
        ViewBag.Types = await _context.PokemonTypes.ToListAsync();
        return View();
    }

    // POST: Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Pokemon pokemon, int[]? SelectedTypes)
    {
        if (ModelState.IsValid)
        {
            // Attach types if selected
            if (SelectedTypes != null)
            {
                pokemon.Types = await _context.PokemonTypes
                    .Where(t => SelectedTypes.Contains(t.Id)).ToListAsync();
            }

            // Link StatValues to Pokemon
            foreach (var statValue in pokemon.StatValues)
            {
                statValue.Pokemon = pokemon;
            }

            _context.Pokemons.Add(pokemon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Repopulate ViewBags if form invalid
        ViewBag.GenerationsList = new SelectList(await _context.Generations.ToListAsync(), "Id", "Name");
        ViewBag.Stats = await _context.PokemonStats.ToListAsync();
        ViewBag.Types = await _context.PokemonTypes.ToListAsync();
        return View(pokemon);
    }



    // GET: Pokemons/Delete/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pokemon = await _context.Pokemons
            .Include(m => m.Generation)
            .Include(m => m.Types)
            .Include(m => m.StatValues)
            .ThenInclude(sv => sv.PokemonStat)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (pokemon == null)
        {
            return NotFound();
        }

        return View(pokemon);
    }






    // GET: Pokemons/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        ViewBag.GenerationsList = new SelectList(await _context.Generations.ToListAsync(), "Id", "Name");
        ViewBag.Stats = await _context.PokemonStats.ToListAsync();
        ViewBag.Types = await _context.PokemonTypes.ToListAsync();
        

        var pokemon = await _context.Pokemons
            .Include(t=>t.Types)
            .Include(p => p.StatValues)
            .FirstOrDefaultAsync(p => p.Id == id);

        //retrieve the list of PokemonType's Id from that specific pokemon
        ViewBag.OldSelectedTypeIds = pokemon.Types.Select(t => t.Id).ToList();

        //retrieve a list of PokemonStatValue object from that specific pokemon
        ViewBag.OldStatValues = pokemon.StatValues.ToList();

        if (pokemon == null)
        {
            return NotFound();
        }
        return View(pokemon);
    }

    /*  
    EF Core tracks entities by their primary key.
    In your case, you already have an existingPokemon loaded from the database (EF is tracking it).
    Then, in your[HttpPost] Edit action, MVC model binding creates a new Pokemon object (pokemon), which also has the same primary key.
    If you try _context.Update(pokemon) or try attaching pokemon directly, 
    EF sees two entities with the same key(Id) and throws the duplicate tracking exception.
    */

    /*
     A many-to-many in EF Core is stored in a join table(PokemonPokemonType).
     EF Core doesn’t automatically “sync” the posted checkboxes(SelectedTypes) with what’s already in the join table.
     If you just add new types without clearing, EF would try to insert duplicates into the join table → PK violation(the error you saw earlier).
    */


    /*
     PokemonPokemonType

     Existing Join Table: Electric and water

    PokemonId | TypeId
    ------------------
        1     |   1     <- Electric
        1     |   3     <- Water

    Attempt to Insert: Electric and flying

    PokemonId | TypeId
    ------------------
        1     |   1     <- Duplicate! ❌ (1,1) composite primary key already exists
        1     |   4     <- Flying ✅

    */


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Pokemon updatedPokemon, int[]? NewSelectedTypes)
    {
        // Fetch the tracked entity
        var editingPokemon = await _context.Pokemons
            .Include(p => p.Types)
            .Include(p => p.StatValues)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (editingPokemon == null) return NotFound();

        try
        {
            editingPokemon.Name = updatedPokemon.Name;
            editingPokemon.Index = updatedPokemon.Index;
            editingPokemon.GenerationId = updatedPokemon.GenerationId;
            editingPokemon.IsLegendary = updatedPokemon.IsLegendary;
            // Clear old types
            editingPokemon.Types.Clear();

            // Add newly selected types
            if (NewSelectedTypes != null)
            {
                var typesToAdd = await _context.PokemonTypes
                    .Where(t => NewSelectedTypes.Contains(t.Id))
                    .ToListAsync();

                foreach (var type in typesToAdd)
                {
                    editingPokemon.Types.Add(type);
                }
            }

            foreach (var updatedStatValue in updatedPokemon.StatValues)
            {
                var editingStateValue = editingPokemon.StatValues
                    .FirstOrDefault(x => x.PokemonStatId == updatedStatValue.PokemonStatId);

                if (editingStateValue != null)
                {
                    editingStateValue.Value = updatedStatValue.Value; // just update
                }
                else
                {
                    editingPokemon.StatValues.Add(updatedStatValue); // new stat added
                }
            }

    

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PokemonExists(id))
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

    private bool PokemonExists(int id)
    {
        return _context.Pokemons.Any(e => e.Id == id);
    }


















    // GET: Pokemons/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pokemon = await _context.Pokemons
            .Include(m=>m.Generation)
            .Include(m=>m.Types)
            .Include(m=>m.StatValues)
            .ThenInclude(sv => sv.PokemonStat)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (pokemon == null)
        {
            return NotFound();
        }

        return View(pokemon);
    }

    // POST: Pokemons/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var pokemon = await _context.Pokemons.FindAsync(id);
        if (pokemon != null)
        {
            _context.Pokemons.Remove(pokemon);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

  
}

