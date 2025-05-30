using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class AddDeductionLevelTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeductionLevel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IndividualDeduction = table.Column<double>(type: "double precision", nullable: false),
                    DependentDeduction = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeductionLevel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeductionLevel");
        }
    }
}
