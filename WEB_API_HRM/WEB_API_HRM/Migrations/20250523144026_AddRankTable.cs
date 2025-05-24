using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class AddRankTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    PriorityLevel = table.Column<int>(type: "integer", nullable: false),
                    RankName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ranks");
        }
    }
}
