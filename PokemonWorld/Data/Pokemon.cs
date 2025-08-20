using System.ComponentModel.DataAnnotations;

namespace PokemonWorld.Data
{
    public class Pokemon
    {
        public int Id { get; set; }

        [Required]
        public int Index { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int GenerationId { get; set; }
        public Generation? Generation { get; set; }

        public bool IsLegendary { get; set; }

        public List<PokemonType> Types { get; set; } = new();
        public List<PokemonStatValue> StatValues { get; set; } = new();
    }
}
