using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class AddBranchTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branchs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    BranchName = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branchs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchDepartment",
                columns: table => new
                {
                    DepartmentId = table.Column<string>(type: "text", nullable: false),
                    BranchId = table.Column<string>(type: "text", nullable: false),
                    BranchModelId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchDepartment", x => new { x.BranchId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_BranchDepartment_Branchs_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchDepartment_Branchs_BranchModelId",
                        column: x => x.BranchModelId,
                        principalTable: "Branchs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BranchDepartment_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchDepartment_BranchModelId",
                table: "BranchDepartment",
                column: "BranchModelId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchDepartment_DepartmentId",
                table: "BranchDepartment",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchDepartment");

            migrationBuilder.DropTable(
                name: "Branchs");
        }
    }
}
