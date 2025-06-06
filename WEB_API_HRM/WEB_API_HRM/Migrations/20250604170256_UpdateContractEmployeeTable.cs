using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class UpdateContractEmployeeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractEmployees_BasicSettingSalary_BasicSettingSalaryId",
                table: "ContractEmployees");

            migrationBuilder.DropIndex(
                name: "IX_ContractEmployees_BasicSettingSalaryId",
                table: "ContractEmployees");

            migrationBuilder.DropColumn(
                name: "BasicSettingSalaryId",
                table: "ContractEmployees");

            migrationBuilder.AddColumn<double>(
                name: "DayWorkStandard",
                table: "ContractEmployees",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HourWorkStandard",
                table: "ContractEmployees",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HourlySalary",
                table: "ContractEmployees",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayWorkStandard",
                table: "ContractEmployees");

            migrationBuilder.DropColumn(
                name: "HourWorkStandard",
                table: "ContractEmployees");

            migrationBuilder.DropColumn(
                name: "HourlySalary",
                table: "ContractEmployees");

            migrationBuilder.AddColumn<string>(
                name: "BasicSettingSalaryId",
                table: "ContractEmployees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ContractEmployees_BasicSettingSalaryId",
                table: "ContractEmployees",
                column: "BasicSettingSalaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractEmployees_BasicSettingSalary_BasicSettingSalaryId",
                table: "ContractEmployees",
                column: "BasicSettingSalaryId",
                principalTable: "BasicSettingSalary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
