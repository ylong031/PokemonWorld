using System.ComponentModel.DataAnnotations;

namespace PokemonWorld.Data
{
    public class Generation
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public List<Pokemon> Pokemons { get; set; } = new();
    }
}