using Microsoft.AspNetCore.Mvc.Rendering;
using PokemonWorld.Data;

namespace PokemonWorld.Models
{
    public class PokemonEditViewModel
    {
        public int Id { get; set; }

      
        public int Index { get; set; }

        public int GenerationId { get; set; }

        public bool IsLegendary { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<PokemonTypeViewModel>? AllPokemonTypes { get; set; } = new();

        /*public List<int> SelectedTypes { get; set; } = new();*/

        public List<PokemonStatsViewModel>? AllPokemonStats { get; set; } = new();
    
        public SelectList? AllGenerations { get; set; }

  /*      public List<PokemonStatValueViewModel> OldStatValues { get; set; } = new();*/

        public List<PokemonStatValueViewModel> StatValues { get; set; } = new();

        public List<int> OldSelectedTypeIds {  get; set; } = new();




    }
}
