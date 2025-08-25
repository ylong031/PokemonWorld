using Microsoft.AspNetCore.Mvc.Rendering;
using PokemonWorld.Data;

namespace PokemonWorld.Models
{
    public class PokemonCreateViewModel
    {
        public int Id { get; set; }

      
        public int Index { get; set; }

        public int GenerationId { get; set; }

        public bool IsLegendary { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<PokemonTypeViewModel> PokemonTypes { get; set; } = new();

        public List<int> SelectedTypes { get; set; } = new();

        public List<PokemonStatsViewModel> PokemonStats { get; set; } = new();
    
        public SelectList? Generations { get; set; }

        public List<PokemonStatValueViewModel> StatValues { get; set; } = new();


    }
}
