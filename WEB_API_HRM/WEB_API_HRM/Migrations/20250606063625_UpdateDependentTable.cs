using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class UpdateDependentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dependents_Employees_EmployeeId",
                table: "Dependents");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Dependents",
                newName: "EmployeeCode");

            migrationBuilder.RenameIndex(
                name: "IX_Dependents_EmployeeId",
                table: "Dependents",
                newName: "IX_Dependents_EmployeeCode");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "TaxEmployees",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dependents_Employees_EmployeeCode",
                table: "Dependents",
                column: "EmployeeCode",
                principalTable: "Employees",
                principalColumn: "EmployeeCode",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dependents_Employees_EmployeeCode",
                table: "Dependents");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TaxEmployees");

            migrationBuilder.RenameColumn(
                name: "EmployeeCode",
                table: "Dependents",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Dependents_EmployeeCode",
                table: "Dependents",
                newName: "IX_Dependents_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dependents_Employees_EmployeeId",
                table: "Dependents",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeCode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
