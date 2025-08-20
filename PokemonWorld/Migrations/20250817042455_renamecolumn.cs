using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonWorld.Migrations
{
    /// <inheritdoc />
    public partial class renamecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PokemonIndex",
                table: "Pokemon",
                newName: "Index");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Index",
                table: "Pokemon",
                newName: "PokemonIndex");
        }
    }
}
