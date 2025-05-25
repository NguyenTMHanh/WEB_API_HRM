using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchDepartment_Branchs_BranchModelId",
                table: "BranchDepartment");

            migrationBuilder.DropIndex(
                name: "IX_BranchDepartment_BranchModelId",
                table: "BranchDepartment");

            migrationBuilder.DropColumn(
                name: "BranchModelId",
                table: "BranchDepartment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchModelId",
                table: "BranchDepartment",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BranchDepartment_BranchModelId",
                table: "BranchDepartment",
                column: "BranchModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchDepartment_Branchs_BranchModelId",
                table: "BranchDepartment",
                column: "BranchModelId",
                principalTable: "Branchs",
                principalColumn: "Id");
        }
    }
}
