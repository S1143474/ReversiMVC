using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiMvcApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spelers",
                columns: table => new
                {
                    Guid = table.Column<string>(nullable: false),
                    Naam = table.Column<string>(nullable: true),
                    AantalGewonnen = table.Column<int>(nullable: false),
                    AantalVerloren = table.Column<int>(nullable: false),
                    AantalGelijk = table.Column<int>(nullable: false),
                    AantalFicheFliped = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spelers", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spelers");
        }
    }
}
