using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonWorld.Migrations
{
    /// <inheritdoc />
    public partial class renameTypeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "PokemonTypes",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PokemonTypes",
                newName: "Type");
        }
    }
}
