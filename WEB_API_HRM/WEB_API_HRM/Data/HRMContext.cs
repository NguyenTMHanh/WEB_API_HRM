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
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoleModuleActionModel>()
                .HasKey(r => new { r.RoleId, r.ModuleId, r.ActionId });

            modelBuilder.Entity<RoleModuleActionModel>()
                .HasOne(r => r.Module)
                .WithMany()
                .HasForeignKey(r => r.ModuleId);

            modelBuilder.Entity<RoleModuleActionModel>()
                .HasOne(r => r.Action)
                .WithMany()
                .HasForeignKey(r => r.ActionId);

            modelBuilder.Entity<RoleModuleActionModel>()
                .HasOne(r => r.Role)
                .WithMany(r => r.RoleModuleActions) 
                .HasForeignKey(r => r.RoleId)
                .HasPrincipalKey(r => r.Id);

            modelBuilder.Entity<JobTitleModel>()
                .HasOne(j => j.Rank) 
                .WithMany() 
                .HasForeignKey(j => j.RankId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<JobTitleModel>()
                .HasOne(j => j.Role) 
                .WithMany() 
                .HasForeignKey(j => j.RoleId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}