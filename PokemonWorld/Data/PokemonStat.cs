using System.ComponentModel.DataAnnotations;

namespace PokemonWorld.Data
{
    public class PokemonStat
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}