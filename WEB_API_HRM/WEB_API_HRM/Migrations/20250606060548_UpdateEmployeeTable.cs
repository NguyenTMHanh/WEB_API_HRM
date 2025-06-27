using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class UpdateEmployeeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceEmployees_RateInsurances_RateInsuranceId",
                table: "InsuranceEmployees");

            migrationBuilder.DropIndex(
                name: "IX_InsuranceEmployees_RateInsuranceId",
                table: "InsuranceEmployees");

            migrationBuilder.DropColumn(
                name: "MinuteBreakLunch",
                table: "PersonelEmployees");

            migrationBuilder.DropColumn(
                name: "RateInsuranceId",
                table: "InsuranceEmployees");

            migrationBuilder.DropColumn(
                name: "DayWorkStandard",
                table: "ContractEmployees");

            migrationBuilder.DropColumn(
                name: "HourWorkStandard",
                table: "ContractEmployees");

            migrationBuilder.DropColumn(
                name: "HourlySalary",
                table: "ContractEmployees");

            migrationBuilder.DropColumn(
                name: "MoneyBasicSalary",
                table: "ContractEmployees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MinuteBreakLunch",
                table: "PersonelEmployees",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "RateInsuranceId",
                table: "InsuranceEmployees",
                type: "text",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<double>(
                name: "MoneyBasicSalary",
                table: "ContractEmployees",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceEmployees_RateInsuranceId",
                table: "InsuranceEmployees",
                column: "RateInsuranceId");

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceEmployees_RateInsurances_RateInsuranceId",
                table: "InsuranceEmployees",
                column: "RateInsuranceId",
                principalTable: "RateInsurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
