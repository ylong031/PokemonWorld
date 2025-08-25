
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using PokemonWorld.Data;
using PokemonWorld.Models;


public class PokemonsController(ApplicationDbContext _context) : Controller
{



    public async Task<IActionResult> Index(int[]? SelectedTypeIds)
    {
        var pokemons = _context.Pokemons
            .Include(pokemon => pokemon.Generation)
            .Include(pokemon => pokemon.Types)
            .Include(pokemon => pokemon.StatValues)
                .ThenInclude(statvalue => statvalue.PokemonStat)
            .AsQueryable();

        // Filter if any types are selected
        if (SelectedTypeIds != null && SelectedTypeIds.Length > 0)
        {
            /*You want to filter Pokemon where at least one of its types matches a selected type ID.*/

            pokemons = pokemons.Where(pokemons => pokemons.Types.Any(type => SelectedTypeIds.Contains(type.Id)));
        }

        var filteredpokemons = await pokemons.ToListAsync();
        var types = await _context.PokemonTypes.ToListAsync();

        // Map to ViewModels (simplified version, you can expand later)
        var viewModel = new PokemonIndexViewModel
        {
            Pokemons = filteredpokemons.Select(filteredpokemon => new PokemonViewModel
            {
                Id = filteredpokemon.Id,
                Name = filteredpokemon.Name,
                Index = filteredpokemon.Index,
                IsLegendary= filteredpokemon.IsLegendary,
                GenerationName = filteredpokemon.Generation.Name,
                TypeNames = filteredpokemon.Types.Select(t => t.Name).ToList(),
                StatValues = filteredpokemon.StatValues.Select(statvalue => new PokemonStatValueViewModel
                {
                    StatName = statvalue.PokemonStat.Name,
                    Value = statvalue.Value
                    
                }).ToList()

            }).ToList(),

            Types = types.Select(type => new PokemonTypeViewModel
            {
                Id = type.Id,
                Name = type.Name
            }).ToList(),

            SelectedTypeIds = SelectedTypeIds
        };

        return View(viewModel);
    }





    

    // GET: Pokemons/Create
    public async Task<IActionResult> Create()
    {
        var pokemonCreateViewModel = new PokemonCreateViewModel
        {
            PokemonTypes = await _context.PokemonTypes
                .Select(t => new PokemonTypeViewModel { Id = t.Id, Name = t.Name })
                .ToListAsync(),

            PokemonStats = await _context.PokemonStats
                .Select(s => new PokemonStatsViewModel { Id = s.Id, Name = s.Name })
                .ToListAsync(),

            // Pre-fill StatValues with zero for each stat
            StatValues = await _context.PokemonStats
                .Select(s => new PokemonStatValueViewModel
                {
                    PokemonStatId = s.Id,
                    StatName = s.Name,
                    Value = 0
                })
                .ToListAsync(),

            Generations = new SelectList(await _context.Generations.ToListAsync(), "Id", "Name")
        };

        return View(pokemonCreateViewModel);
    }






    // POST: Pokemons/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PokemonCreateViewModel pokemonCreateViewModel)
    {
        if (!ModelState.IsValid)
        {
            foreach(var error in ModelState) 
            {
               if(error.Value.Errors.Count > 0) 
                {
                    Console.WriteLine($"Key: {error.Key}, Error: {error.Value.Errors[0].ErrorMessage}");
                }

            }

            // Re-populate view model for redisplay
            pokemonCreateViewModel.PokemonTypes = await _context.PokemonTypes
                .Select(t => new PokemonTypeViewModel { Id = t.Id, Name = t.Name })
                .ToListAsync();

            pokemonCreateViewModel.StatValues = await _context.PokemonStats
                .Select(s => new PokemonStatValueViewModel
                {
                    PokemonStatId = s.Id,
                    StatName = s.Name,
                    Value = 0
                }).ToListAsync();

            pokemonCreateViewModel.Generations = new SelectList(await _context.Generations.ToListAsync(), "Id", "Name");

            return View(pokemonCreateViewModel);
        }

        // Map ViewModel to Data Model
        var pokemon = new Pokemon
        {
            Index = pokemonCreateViewModel.Index,
            Name = pokemonCreateViewModel.Name,
            GenerationId = pokemonCreateViewModel.GenerationId,
            IsLegendary = pokemonCreateViewModel.IsLegendary,

            // Map selected types
            Types = pokemonCreateViewModel.SelectedTypes != null
                ? await _context.PokemonTypes
                    .Where(t => pokemonCreateViewModel.SelectedTypes.Contains(t.Id))
                    .ToListAsync()
                : new List<PokemonType>(),

            // Map stat values
            StatValues = pokemonCreateViewModel.StatValues
                .Select(sv => new PokemonStatValue
                {
                    PokemonStatId = sv.PokemonStatId,
                    Value = sv.Value
                }).ToList()
        };

        _context.Pokemons.Add(pokemon);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    

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


        PokemonViewModel pokemonViewModel = new PokemonViewModel
        {
            Id = pokemon.Id,
            Index = pokemon.Index,
            Name = pokemon.Name,
            IsLegendary = pokemon.IsLegendary,
            GenerationName = pokemon.Generation.Name,
            TypeNames = pokemon.Types.Select(t => t.Name).ToList(),
            StatValues = pokemon.StatValues.Select(sv => new PokemonStatValueViewModel
            {
                StatName = sv.PokemonStat.Name,
                Value = sv.Value
            }).ToList()


        };


        if (pokemon == null)
        {
            return NotFound();
        }

        return View(pokemonViewModel);
    }






    


    // GET: Pokemons/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pokemon = await _context.Pokemons
          .Include(t => t.Types)
          .Include(p => p.StatValues)
            .ThenInclude(sv => sv.PokemonStat)
          .FirstOrDefaultAsync(p => p.Id == id);


        if (pokemon == null)
        {
            return NotFound();
        }

        /*await async is for database data retrieval*/
        /*do not use await when you are working with in-memory objects*/
        PokemonEditViewModel pokemonEditViewModel = new PokemonEditViewModel
        {
            Id = pokemon.Id,
            Index = pokemon.Index,
            Name = pokemon.Name,
            IsLegendary = pokemon.IsLegendary,
          /*  SelectedTypes= pokemon.Types.Select(t => t.Id).ToList(),*/
            GenerationId = pokemon.GenerationId,
            AllGenerations= new SelectList(await _context.Generations.ToListAsync(), "Id", "Name"),
            AllPokemonTypes=await _context.PokemonTypes.Select(t=>new PokemonTypeViewModel
            {
                Id = t.Id,
                Name = t.Name,

            }).ToListAsync(),
            AllPokemonStats = await _context.PokemonStats.Select(t => new PokemonStatsViewModel
            {
                Id=t.Id,
                Name=t.Name,

                

            }).ToListAsync(),

            OldSelectedTypeIds= pokemon.Types
            .Select(t => t.Id).ToList(),

     
            StatValues = pokemon.StatValues.Select(sv => new PokemonStatValueViewModel
            {
                PokemonStatId = sv.PokemonStatId,
                StatName = sv.PokemonStat.Name,
                Value = sv.Value
            }).ToList(),



        };

        return View(pokemonEditViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int Id, PokemonEditViewModel pokemonEditViewModel, int[]? NewSelectedTypes)
    {

        Pokemon updatedPokemon = new Pokemon()
        {
            Id = pokemonEditViewModel.Id,
            Index = pokemonEditViewModel.Index,
            Name = pokemonEditViewModel.Name,
            IsLegendary = pokemonEditViewModel.IsLegendary,
            GenerationId = pokemonEditViewModel.GenerationId,
            StatValues = pokemonEditViewModel.StatValues.Select(sv => new PokemonStatValue
            {
                PokemonStatId = sv.PokemonStatId,
                Value = sv.Value
            }).ToList()


        };


        // Fetch the tracked entity
        var editingPokemon = await _context.Pokemons
            .Include(p => p.Types)
            .Include(p => p.StatValues)
            .FirstOrDefaultAsync(p => p.Id == Id);

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
                    .FirstOrDefault(statvalue => statvalue.PokemonStatId == updatedStatValue.PokemonStatId);

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
            if (!PokemonExists(Id))
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



        PokemonViewModel pokemonViewModel = new PokemonViewModel
        {
            Id = pokemon.Id,
            Index = pokemon.Index,
            Name = pokemon.Name,
            IsLegendary = pokemon.IsLegendary,
            GenerationName = pokemon.Generation.Name,
            TypeNames = pokemon.Types.Select(t => t.Name).ToList(),
            StatValues = pokemon.StatValues.Select(sv => new PokemonStatValueViewModel
            {
                StatName = sv.PokemonStat.Name,
                Value = sv.Value
            }).ToList()


        };

        return View(pokemonViewModel);
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


/*    // GET: Create
        public async Task<IActionResult> Create()
        {



            ViewBag.GenerationsList = new SelectList(await _context.Generations.ToListAsync(), "Id", "Name");
            ViewBag.Stats = await _context.PokemonStats.ToListAsync();
            ViewBag.Types = await _context.PokemonTypes.ToListAsync();
            return View();
        }*/


/*   // POST: Create
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
}*/


/*    public async Task<IActionResult> Details(int? id)
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
        }*/


// GET: Pokemons/Edit/5
/*  public async Task<IActionResult> Edit(int? id)
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
  }*/

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


/* [HttpPost]
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
 }*/
