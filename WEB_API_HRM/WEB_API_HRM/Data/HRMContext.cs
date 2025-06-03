using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Data
{
    public class HRMContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public HRMContext(DbContextOptions<HRMContext> options) : base(options)
        {
        }

        #region DbSets
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ModuleModel> Modules { get; set; }
        public DbSet<ActionModel> Actions { get; set; }
        public DbSet<RoleModuleActionModel> RoleModuleActions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<RankModel> Ranks { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<JobTitleModel> JobTitles { get; set; }
        public DbSet<PositionModel> Positions { get; set; }
        public DbSet<BranchModel> Branchs { get; set; }
        public DbSet<BranchDepartmentModel> BranchDepartment { get; set; }
        public DbSet<JobTypeModel> JobTypes { get; set; }
        public DbSet<HolidayModel> Holidays { get; set; }
        public DbSet<CheckInOutSettingModel> CheckInOutSettings { get; set; }
        public DbSet<RateInsuranceModel> RateInsurances { get; set; }
        public DbSet<TaxRateProgressionModel> TaxRateProgressions { get; set; }
        public DbSet<DeductionLevelModel> DeductionLevel { get; set; }
        public DbSet<SalaryCoefficientModel> SalaryCoefficients { get; set; }
        public DbSet<AllowanceModel> Allowances { get; set; }
        public DbSet<MinimumWageAreaModel> MinimumWageAreas { get; set; }
        public DbSet<BasicSettingSalaryModel> BasicSettingSalary { get; set; }
        public DbSet<TaxEmployeeModel> TaxEmployees { get; set; }
        public DbSet<InsuranceEmployeeModel> InsuranceEmployees { get; set; }
        public DbSet<ContractEmployeeModel> ContractEmployees { get; set; }
        public DbSet<PersonelEmployeeModel> PersonelEmployees { get; set; }
        public DbSet<PersonalEmployeeModel> PersonalEmployees { get; set; }
        public DbSet<DependentModel> Dependents { get; set; }
        public DbSet<EmployeeAllowance> EmployeeAllowances { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // RoleModuleActionModel Configuration
            modelBuilder.Entity<RoleModuleActionModel>()
                .HasKey(r => new { r.RoleId, r.ModuleId, r.ActionId });
            modelBuilder.Entity<RoleModuleActionModel>()
                .HasOne(r => r.Module).WithMany().HasForeignKey(r => r.ModuleId);
            modelBuilder.Entity<RoleModuleActionModel>()
                .HasOne(r => r.Action).WithMany().HasForeignKey(r => r.ActionId);
            modelBuilder.Entity<RoleModuleActionModel>()
                .HasOne(r => r.Role).WithMany(r => r.RoleModuleActions).HasForeignKey(r => r.RoleId);

            // JobTitleModel Configuration
            modelBuilder.Entity<JobTitleModel>()
                .HasOne(j => j.Rank).WithMany().HasForeignKey(j => j.RankId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<JobTitleModel>()
                .HasOne(j => j.Role).WithMany().HasForeignKey(j => j.RoleId).OnDelete(DeleteBehavior.Restrict);

            // PositionModel Configuration
            modelBuilder.Entity<PositionModel>()
                .HasOne(j => j.Department).WithMany().HasForeignKey(j => j.DepartmentId).OnDelete(DeleteBehavior.Restrict);

            // BranchDepartmentModel Configuration
            modelBuilder.Entity<BranchDepartmentModel>()
                .HasKey(b => new { b.BranchId, b.DepartmentId });
            modelBuilder.Entity<BranchDepartmentModel>()
                .HasOne(b => b.Branch)
                .WithMany(b => b.BranchDepartment)
                .HasForeignKey(b => b.BranchId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BranchDepartmentModel>()
                .HasOne(b => b.Department)
                .WithMany()
                .HasForeignKey(b => b.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // SalaryCoefficientModel Configuration
            modelBuilder.Entity<SalaryCoefficientModel>()
                .HasOne(j => j.Position).WithMany().HasForeignKey(j => j.PositionId).OnDelete(DeleteBehavior.Restrict);

            // TaxEmployeeModel Configuration
            modelBuilder.Entity<TaxEmployeeModel>()
                .HasKey(t => t.EmployeeCode); // Assuming EmployeeCode is the primary key
            modelBuilder.Entity<TaxEmployeeModel>()
                .HasOne(t => t.Employee)
                .WithOne()
                .HasForeignKey<TaxEmployeeModel>(t => t.EmployeeCode)
                .OnDelete(DeleteBehavior.Cascade);

            // InsuranceEmployeeModel Configuration
            modelBuilder.Entity<InsuranceEmployeeModel>()
                .HasKey(i => i.Id);
            modelBuilder.Entity<InsuranceEmployeeModel>()
                .HasOne(i => i.RateInsurance)
                .WithMany()
                .HasForeignKey(i => i.RateInsuranceId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InsuranceEmployeeModel>()
                .HasOne(i => i.Employee)
                .WithOne()
                .HasForeignKey<InsuranceEmployeeModel>(i => i.EmployeeCode)
                .OnDelete(DeleteBehavior.Cascade);

            // ContractEmployeeModel Configuration
            modelBuilder.Entity<ContractEmployeeModel>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<ContractEmployeeModel>()
                .HasOne(c => c.BasicSettingSalary)
                .WithMany()
                .HasForeignKey(c => c.BasicSettingSalaryId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ContractEmployeeModel>()
                .HasOne(c => c.SalaryCoefficient)
                .WithMany()
                .HasForeignKey(c => c.SalaryCoefficientId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ContractEmployeeModel>()
                .HasOne(c => c.Employee)
                .WithOne()
                .HasForeignKey<ContractEmployeeModel>(c => c.EmployeeCode)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelEmployeeModel Configuration
            modelBuilder.Entity<PersonelEmployeeModel>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<PersonelEmployeeModel>()
                .HasOne(p => p.Branch)
                .WithMany()
                .HasForeignKey(p => p.BranchId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PersonelEmployeeModel>()
                .HasOne(p => p.Department)
                .WithMany()
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PersonelEmployeeModel>()
                .HasOne(p => p.JobTitle)
                .WithMany()
                .HasForeignKey(p => p.JobTitleId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PersonelEmployeeModel>()
                .HasOne(p => p.Rank)
                .WithMany()
                .HasForeignKey(p => p.RankId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PersonelEmployeeModel>()
                .HasOne(p => p.Position)
                .WithMany()
                .HasForeignKey(p => p.PositionId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PersonelEmployeeModel>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PersonelEmployeeModel>()
                .HasOne(p => p.Role)
                .WithMany()
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PersonelEmployeeModel>()
                .HasOne(p => p.Employee)
                .WithOne()
                .HasForeignKey<PersonelEmployeeModel>(p => p.EmployeeCode)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonalEmployeeModel Configuration
            modelBuilder.Entity<PersonalEmployeeModel>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<PersonalEmployeeModel>()
                .HasOne(p => p.Employee)
                .WithOne()
                .HasForeignKey<PersonalEmployeeModel>(p => p.EmployeeCode)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false); 

            // DependentModel Configuration
            modelBuilder.Entity<DependentModel>()
                .HasKey(d => d.Id);
            modelBuilder.Entity<DependentModel>()
                .HasOne(d => d.Employee)
                .WithMany()
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // EmployeeAllowance Configuration
            modelBuilder.Entity<EmployeeAllowance>()
                .HasKey(ea => new { ea.AllowanceId, ea.EmployeeCode });
            modelBuilder.Entity<EmployeeAllowance>()
                .HasOne(ea => ea.Allowance)
                .WithMany()
                .HasForeignKey(ea => ea.AllowanceId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<EmployeeAllowance>()
                .HasOne(ea => ea.Employee)
                .WithMany()
                .HasForeignKey(ea => ea.EmployeeCode)
                .OnDelete(DeleteBehavior.Cascade);

            // EmployeeModel Configuration
            modelBuilder.Entity<EmployeeModel>()
                .HasKey(e => e.EmployeeCode);
        }
    }
}