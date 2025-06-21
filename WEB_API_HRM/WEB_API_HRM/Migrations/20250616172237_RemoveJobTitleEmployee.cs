using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class RemoveJobTitleEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonelEmployees_JobTitles_JobTitleId",
                table: "PersonelEmployees");

            migrationBuilder.DropIndex(
                name: "IX_PersonelEmployees_JobTitleId",
                table: "PersonelEmployees");

            migrationBuilder.DropColumn(
                name: "JobTitleId",
                table: "PersonelEmployees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobTitleId",
                table: "PersonelEmployees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelEmployees_JobTitleId",
                table: "PersonelEmployees",
                column: "JobTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonelEmployees_JobTitles_JobTitleId",
                table: "PersonelEmployees",
                column: "JobTitleId",
                principalTable: "JobTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
