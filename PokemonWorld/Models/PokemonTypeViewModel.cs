using System.ComponentModel.DataAnnotations;
namespace PokemonWorld.Models
{
    public class PokemonTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}