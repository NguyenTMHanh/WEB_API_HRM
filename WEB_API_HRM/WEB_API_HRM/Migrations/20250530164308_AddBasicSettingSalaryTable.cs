using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class AddBasicSettingSalaryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasicSettingSalary",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    HourlySalary = table.Column<double>(type: "double precision", nullable: false),
                    HourWorkStandard = table.Column<double>(type: "double precision", nullable: false),
                    DayWorkStandard = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicSettingSalary", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasicSettingSalary");
        }
    }
}
