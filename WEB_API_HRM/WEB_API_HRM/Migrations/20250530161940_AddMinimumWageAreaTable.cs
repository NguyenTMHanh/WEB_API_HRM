using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class AddMinimumWageAreaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MinimumWageAreas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    NameArea = table.Column<string>(type: "text", nullable: false),
                    MoneyMinimumWageArea = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinimumWageAreas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MinimumWageAreas");
        }
    }
}
