
using System.ComponentModel.DataAnnotations;

namespace PokemonWorld.Models
{
    public class PokemonReadOnlyViewModel
    {
        public int Id { get; set; }

        [Required]
        public int Index { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public GenerationViewModel? Generation { get; set; }

        public bool IsLegendary { get; set; }

        public List<PokemonTypeViewModel> Types { get; set; } = new();
        public List<PokemonStatValueViewModel> StatValues { get; set; } = new();
    }
}
