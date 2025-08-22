
using PokemonWorld.Data;
using System.ComponentModel.DataAnnotations;

namespace PokemonWorld.Models
{
 
        public class PokemonIndexViewModel
        {
            public List<PokemonViewModel> Pokemons { get; set; } = new();
            public List<PokemonTypeViewModel> Types { get; set; } = new();
            public int[]? SelectedTypeIds { get; set; }
        }
    
}
