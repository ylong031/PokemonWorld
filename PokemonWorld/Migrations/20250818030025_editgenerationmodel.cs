using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonWorld.Migrations
{
    /// <inheritdoc />
    public partial class editgenerationmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PokemonStatValue_PokemonStats_PokemonStatId",
                table: "PokemonStatValue");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonStatValue_Pokemon_PokemonId",
                table: "PokemonStatValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PokemonStatValue",
                table: "PokemonStatValue");

            migrationBuilder.RenameTable(
                name: "PokemonStatValue",
                newName: "PokemonStatValues");

            migrationBuilder.RenameColumn(
                name: "_Generation",
                table: "Generations",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_PokemonStatValue_PokemonStatId",
                table: "PokemonStatValues",
                newName: "IX_PokemonStatValues_PokemonStatId");

            migrationBuilder.RenameIndex(
                name: "IX_PokemonStatValue_PokemonId",
                table: "PokemonStatValues",
                newName: "IX_PokemonStatValues_PokemonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PokemonStatValues",
                table: "PokemonStatValues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonStatValues_PokemonStats_PokemonStatId",
                table: "PokemonStatValues",
                column: "PokemonStatId",
                principalTable: "PokemonStats",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PokemonStatValues_PokemonStats_PokemonStatId",
                table: "PokemonStatValues");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonStatValues_Pokemon_PokemonId",
                table: "PokemonStatValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PokemonStatValues",
                table: "PokemonStatValues");

            migrationBuilder.RenameTable(
                name: "PokemonStatValues",
                newName: "PokemonStatValue");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Generations",
                newName: "_Generation");

            migrationBuilder.RenameIndex(
                name: "IX_PokemonStatValues_PokemonStatId",
                table: "PokemonStatValue",
                newName: "IX_PokemonStatValue_PokemonStatId");

            migrationBuilder.RenameIndex(
                name: "IX_PokemonStatValues_PokemonId",
                table: "PokemonStatValue",
                newName: "IX_PokemonStatValue_PokemonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PokemonStatValue",
                table: "PokemonStatValue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonStatValue_PokemonStats_PokemonStatId",
                table: "PokemonStatValue",
                column: "PokemonStatId",
                principalTable: "PokemonStats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonStatValue_Pokemon_PokemonId",
                table: "PokemonStatValue",
                column: "PokemonId",
                principalTable: "Pokemon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
