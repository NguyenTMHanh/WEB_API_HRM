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
        public DbSet<BranchDepartmentModel> BranchDepartment {get; set;}
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Các cấu hình hiện có cho các thực thể khác...
            modelBuilder.Entity<RoleModuleActionModel>()
                .HasKey(r => new { r.RoleId, r.ModuleId, r.ActionId });
            modelBuilder.Entity<RoleModuleActionModel>()
                .HasOne(r => r.Module).WithMany().HasForeignKey(r => r.ModuleId);
            modelBuilder.Entity<RoleModuleActionModel>()
                .HasOne(r => r.Action).WithMany().HasForeignKey(r => r.ActionId);
            modelBuilder.Entity<RoleModuleActionModel>()
                .HasOne(r => r.Role).WithMany(r => r.RoleModuleActions).HasForeignKey(r => r.RoleId);

            modelBuilder.Entity<JobTitleModel>()
                .HasOne(j => j.Rank).WithMany().HasForeignKey(j => j.RankId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<JobTitleModel>()
                .HasOne(j => j.Role).WithMany().HasForeignKey(j => j.RoleId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PositionModel>()
                .HasOne(j => j.Department).WithMany().HasForeignKey(j => j.DepartmentId).OnDelete(DeleteBehavior.Restrict);

            // Cấu hình BranchDepartment như bảng trung gian nhiều-nhiều
            modelBuilder.Entity<BranchDepartmentModel>()
                .HasKey(b => new { b.BranchId, b.DepartmentId });

            modelBuilder.Entity<BranchDepartmentModel>()
                .HasOne(b => b.Branch)
                .WithMany(b => b.BranchDepartment) // Liên kết với tập hợp trong BranchModel
                .HasForeignKey(b => b.BranchId)
                .OnDelete(DeleteBehavior.Cascade); // hoặc Restrict, tùy theo nhu cầu

            modelBuilder.Entity<BranchDepartmentModel>()
                .HasOne(b => b.Department)
                .WithMany() 
                .HasForeignKey(b => b.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade); // hoặc Restrict, tùy theo nhu cầu
        }
    }
}