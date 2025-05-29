using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class AddRateInsuranceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RateInsurances",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    bhytBusinessRate = table.Column<double>(type: "double precision", nullable: false),
                    bhytEmpRate = table.Column<double>(type: "double precision", nullable: false),
                    bhxhBusinessRate = table.Column<double>(type: "double precision", nullable: false),
                    bhxhEmpRate = table.Column<double>(type: "double precision", nullable: false),
                    bhtnBusinessRate = table.Column<double>(type: "double precision", nullable: false),
                    bhtnEmpRate = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateInsurances", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RateInsurances");
        }
    }
}
