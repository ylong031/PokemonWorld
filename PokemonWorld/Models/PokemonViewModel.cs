namespace PokemonWorld.Models
{
    public class PokemonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string GenerationName { get; set; }

        public bool IsLegendary { get; set; }
        public List<string> TypeNames { get; set; }
        public List<PokemonStatValueViewModel> StatValues { get; set; }
    }
}