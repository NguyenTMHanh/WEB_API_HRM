using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class AddAllowanceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Allowances",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    NameAllowance = table.Column<string>(type: "text", nullable: false),
                    MoneyAllowance = table.Column<double>(type: "double precision", nullable: false),
                    TypeAllowance = table.Column<string>(type: "text", nullable: false),
                    MoneyAllowanceNoTax = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allowances", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allowances");
        }
    }
}
