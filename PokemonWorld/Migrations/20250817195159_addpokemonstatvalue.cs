using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonWorld.Migrations
{
    /// <inheritdoc />
    public partial class addpokemonstatvalue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PokemonStats_Pokemon_PokemonId",
                table: "PokemonStats");

            migrationBuilder.DropIndex(
                name: "IX_PokemonStats_PokemonId",
                table: "PokemonStats");

            migrationBuilder.DropColumn(
                name: "PokemonId",
                table: "PokemonStats");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "PokemonStats");

            migrationBuilder.CreateTable(
                name: "PokemonStatValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PokemonId = table.Column<int>(type: "int", nullable: false),
                    PokemonStatId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonStatValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PokemonStatValue_PokemonStats_PokemonStatId",
                        column: x => x.PokemonStatId,
                        principalTable: "PokemonStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonStatValue_Pokemon_PokemonId",
                        column: x => x.PokemonId,
                        principalTable: "Pokemon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PokemonStatValue_PokemonId",
                table: "PokemonStatValue",
                column: "PokemonId");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonStatValue_PokemonStatId",
                table: "PokemonStatValue",
                column: "PokemonStatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PokemonStatValue");

            migrationBuilder.AddColumn<int>(
                name: "PokemonId",
                table: "PokemonStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Power",
                table: "PokemonStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PokemonStats_PokemonId",
                table: "PokemonStats",
                column: "PokemonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonStats_Pokemon_PokemonId",
                table: "PokemonStats",
                column: "PokemonId",
                principalTable: "Pokemon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
