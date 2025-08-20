namespace PokemonWorld.Data
{
    public class PokemonStatValue
    {
        public int Id { get; set; }
        public int Value { get; set; }

        public int PokemonId { get; set; }
        public Pokemon? Pokemon { get; set; }

        public int PokemonStatId { get; set; }
        public PokemonStat? PokemonStat { get; set; }
    }
}
