using System.ComponentModel.DataAnnotations;
namespace PokemonWorld.Models
{
    public class GenerationViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}