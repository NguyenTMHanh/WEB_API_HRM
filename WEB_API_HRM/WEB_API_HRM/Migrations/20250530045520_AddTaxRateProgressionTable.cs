using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class AddTaxRateProgressionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxRateProgressions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TaxableIncome = table.Column<double>(type: "double precision", nullable: false),
                    TaxRate = table.Column<double>(type: "double precision", nullable: false),
                    Progression = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRateProgressions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxRateProgressions");
        }
    }
}
