using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class AddSalaryCoeffidientTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalaryCoefficients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SalaryCoefficient = table.Column<double>(type: "double precision", nullable: false),
                    PositionId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryCoefficients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryCoefficients_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryCoefficients_PositionId",
                table: "SalaryCoefficients",
                column: "PositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaryCoefficients");
        }
    }
}
