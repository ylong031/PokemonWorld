using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonWorld.Migrations
{
    /// <inheritdoc />
    public partial class editdbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pokemon_Generations_GenerationId",
                table: "Pokemon");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonPokemonType_Pokemon_PokemonsId",
                table: "PokemonPokemonType");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonStatValues_Pokemon_PokemonId",
                table: "PokemonStatValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pokemon",
                table: "Pokemon");

            migrationBuilder.RenameTable(
                name: "Pokemon",
                newName: "Pokemons");

            migrationBuilder.RenameIndex(
                name: "IX_Pokemon_GenerationId",
                table: "Pokemons",
                newName: "IX_Pokemons_GenerationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pokemons",
                table: "Pokemons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonPokemonType_Pokemons_PokemonsId",
                table: "PokemonPokemonType",
                column: "PokemonsId",
                principalTable: "Pokemons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pokemons_Generations_GenerationId",
                table: "Pokemons",
                column: "GenerationId",
                principalTable: "Generations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonStatValues_Pokemons_PokemonId",
                table: "PokemonStatValues",
                column: "PokemonId",
                principalTable: "Pokemons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PokemonPokemonType_Pokemons_PokemonsId",
                table: "PokemonPokemonType");

            migrationBuilder.DropForeignKey(
                name: "FK_Pokemons_Generations_GenerationId",
                table: "Pokemons");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonStatValues_Pokemons_PokemonId",
                table: "PokemonStatValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pokemons",
                table: "Pokemons");

            migrationBuilder.RenameTable(
                name: "Pokemons",
                newName: "Pokemon");

            migrationBuilder.RenameIndex(
                name: "IX_Pokemons_GenerationId",
                table: "Pokemon",
                newName: "IX_Pokemon_GenerationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pokemon",
                table: "Pokemon",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pokemon_Generations_GenerationId",
                table: "Pokemon",
                column: "GenerationId",
                principalTable: "Generations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonPokemonType_Pokemon_PokemonsId",
                table: "PokemonPokemonType",
                column: "PokemonsId",
                principalTable: "Pokemon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonStatValues_Pokemon_PokemonId",
                table: "PokemonStatValues",
                column: "PokemonId",
                principalTable: "Pokemon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
