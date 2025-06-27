using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API_HRM.Migrations
{
    public partial class AddEmployeeTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeCode);
                });

            migrationBuilder.CreateTable(
                name: "ContractEmployees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ContractCode = table.Column<string>(type: "text", nullable: false),
                    TypeContract = table.Column<string>(type: "text", nullable: false),
                    DateStartContract = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateEndContract = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ContractStatus = table.Column<string>(type: "text", nullable: false),
                    BasicSettingSalaryId = table.Column<string>(type: "text", nullable: false),
                    MoneyBasicSalary = table.Column<double>(type: "double precision", nullable: false),
                    SalaryCoefficientId = table.Column<string>(type: "text", nullable: false),
                    EmployeeCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractEmployees_BasicSettingSalary_BasicSettingSalaryId",
                        column: x => x.BasicSettingSalaryId,
                        principalTable: "BasicSettingSalary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractEmployees_Employees_EmployeeCode",
                        column: x => x.EmployeeCode,
                        principalTable: "Employees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractEmployees_SalaryCoefficients_SalaryCoefficientId",
                        column: x => x.SalaryCoefficientId,
                        principalTable: "SalaryCoefficients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dependents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    RegisterDependentStatus = table.Column<string>(type: "text", nullable: false),
                    TaxCode = table.Column<string>(type: "text", nullable: false),
                    NameDependent = table.Column<string>(type: "text", nullable: false),
                    DayOfBirthDependent = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Relationship = table.Column<string>(type: "text", nullable: false),
                    EvidencePath = table.Column<string>(type: "text", nullable: false),
                    EmployeeId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dependents_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceEmployees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CodeBHYT = table.Column<string>(type: "text", nullable: false),
                    RateInsuranceId = table.Column<string>(type: "text", nullable: false),
                    RegisterMedical = table.Column<string>(type: "text", nullable: false),
                    DateStartParticipateBHYT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HasBHXH = table.Column<bool>(type: "boolean", nullable: false),
                    CodeBHXH = table.Column<string>(type: "text", nullable: false),
                    DateStartParticipateBHXH = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateStartParticipateBHTN = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InsuranceStatus = table.Column<string>(type: "text", nullable: false),
                    DateEndParticipateInsurance = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EmployeeCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceEmployees_Employees_EmployeeCode",
                        column: x => x.EmployeeCode,
                        principalTable: "Employees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceEmployees_RateInsurances_RateInsuranceId",
                        column: x => x.RateInsuranceId,
                        principalTable: "RateInsurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonalEmployees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    NameEmployee = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Nationality = table.Column<string>(type: "text", nullable: false),
                    Ethnicity = table.Column<string>(type: "text", nullable: false),
                    NumberIdentification = table.Column<string>(type: "text", nullable: false),
                    DateIssueIdentification = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlaceIssueIdentification = table.Column<string>(type: "text", nullable: false),
                    FrontIdentificationPath = table.Column<string>(type: "text", nullable: false),
                    BackIdentificationPath = table.Column<string>(type: "text", nullable: false),
                    ProvinceResidence = table.Column<string>(type: "text", nullable: false),
                    DistrictResidence = table.Column<string>(type: "text", nullable: false),
                    WardResidence = table.Column<string>(type: "text", nullable: false),
                    HouseNumberResidence = table.Column<string>(type: "text", nullable: false),
                    ProvinceContact = table.Column<string>(type: "text", nullable: false),
                    DistrictContact = table.Column<string>(type: "text", nullable: false),
                    WardContact = table.Column<string>(type: "text", nullable: false),
                    HouseNumberContact = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    BankNumber = table.Column<string>(type: "text", nullable: false),
                    NameBank = table.Column<string>(type: "text", nullable: false),
                    BranchBank = table.Column<string>(type: "text", nullable: false),
                    EmployeeCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalEmployees_Employees_EmployeeCode",
                        column: x => x.EmployeeCode,
                        principalTable: "Employees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelEmployees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DateJoinCompany = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BranchId = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<string>(type: "text", nullable: false),
                    JobTitleId = table.Column<string>(type: "text", nullable: false),
                    RankId = table.Column<string>(type: "text", nullable: false),
                    PositionId = table.Column<string>(type: "text", nullable: false),
                    ManagerId = table.Column<string>(type: "text", nullable: false),
                    JobTypeId = table.Column<string>(type: "text", nullable: false),
                    MinuteBreakLunch = table.Column<double>(type: "double precision", nullable: false),
                    AvatarPath = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    EmployeeCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelEmployees_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelEmployees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelEmployees_Branchs_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelEmployees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelEmployees_Employees_EmployeeCode",
                        column: x => x.EmployeeCode,
                        principalTable: "Employees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonelEmployees_JobTitles_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "JobTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelEmployees_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelEmployees_Ranks_RankId",
                        column: x => x.RankId,
                        principalTable: "Ranks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaxEmployees",
                columns: table => new
                {
                    EmployeeCode = table.Column<string>(type: "text", nullable: false),
                    HasTaxCode = table.Column<bool>(type: "boolean", nullable: false),
                    TaxCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxEmployees", x => x.EmployeeCode);
                    table.ForeignKey(
                        name: "FK_TaxEmployees_Employees_EmployeeCode",
                        column: x => x.EmployeeCode,
                        principalTable: "Employees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAllowances",
                columns: table => new
                {
                    AllowanceId = table.Column<string>(type: "text", nullable: false),
                    EmployeeCode = table.Column<string>(type: "text", nullable: false),
                    ContractEmployeeModelId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAllowances", x => new { x.AllowanceId, x.EmployeeCode });
                    table.ForeignKey(
                        name: "FK_EmployeeAllowances_Allowances_AllowanceId",
                        column: x => x.AllowanceId,
                        principalTable: "Allowances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeAllowances_ContractEmployees_ContractEmployeeModelId",
                        column: x => x.ContractEmployeeModelId,
                        principalTable: "ContractEmployees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeAllowances_Employees_EmployeeCode",
                        column: x => x.EmployeeCode,
                        principalTable: "Employees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractEmployees_BasicSettingSalaryId",
                table: "ContractEmployees",
                column: "BasicSettingSalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractEmployees_EmployeeCode",
                table: "ContractEmployees",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractEmployees_SalaryCoefficientId",
                table: "ContractEmployees",
                column: "SalaryCoefficientId");

            migrationBuilder.CreateIndex(
                name: "IX_Dependents_EmployeeId",
                table: "Dependents",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAllowances_ContractEmployeeModelId",
                table: "EmployeeAllowances",
                column: "ContractEmployeeModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAllowances_EmployeeCode",
                table: "EmployeeAllowances",
                column: "EmployeeCode");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceEmployees_EmployeeCode",
                table: "InsuranceEmployees",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceEmployees_RateInsuranceId",
                table: "InsuranceEmployees",
                column: "RateInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalEmployees_EmployeeCode",
                table: "PersonalEmployees",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonelEmployees_BranchId",
                table: "PersonelEmployees",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelEmployees_DepartmentId",
                table: "PersonelEmployees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelEmployees_EmployeeCode",
                table: "PersonelEmployees",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonelEmployees_JobTitleId",
                table: "PersonelEmployees",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelEmployees_PositionId",
                table: "PersonelEmployees",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelEmployees_RankId",
                table: "PersonelEmployees",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelEmployees_RoleId",
                table: "PersonelEmployees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelEmployees_UserId",
                table: "PersonelEmployees",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dependents");

            migrationBuilder.DropTable(
                name: "EmployeeAllowances");

            migrationBuilder.DropTable(
                name: "InsuranceEmployees");

            migrationBuilder.DropTable(
                name: "PersonalEmployees");

            migrationBuilder.DropTable(
                name: "PersonelEmployees");

            migrationBuilder.DropTable(
                name: "TaxEmployees");

            migrationBuilder.DropTable(
                name: "ContractEmployees");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
