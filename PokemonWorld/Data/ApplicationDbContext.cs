using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PokemonWorld.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pokemon> Pokemons { get; set; }

        public DbSet<PokemonType> PokemonTypes { get; set; }

        public DbSet<PokemonStat> PokemonStats { get; set; }

        public DbSet<Generation> Generations { get; set; }

        public DbSet<PokemonStatValue> PokemonStatValues { get; set; }




    }
}
