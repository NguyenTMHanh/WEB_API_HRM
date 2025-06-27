using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using WEB_API_HRM.Helpers;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(HRMContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                if (!context.Modules.Any())
                {
                    context.Modules.AddRange(
                        new ModuleModel { Id = "profilePersonal", ModuleName = "Hồ sơ cá nhân" },
                        new ModuleModel { Id = "profilePersonel", ModuleName = "Hồ sơ nhân sự" },
                        new ModuleModel { Id = "profileContract", ModuleName = "HDLĐ" },
                        new ModuleModel { Id = "profileInsurance", ModuleName = "Bảo Hiểm" },
                        new ModuleModel { Id = "profileTax", ModuleName = "Thuế TNCN" },
                        new ModuleModel { Id = "HrPersonel", ModuleName = "Danh sách Hồ sơ nhân sự" },                       
                        new ModuleModel { Id = "HrSalary", ModuleName = "Danh sách lương nhân sự" },
                        new ModuleModel { Id = "HrHistoryCheckin", ModuleName = "Danh sách lịch sử chấm công nhân sự" },
                        new ModuleModel { Id = "setting", ModuleName = "Cài đặt thông số hệ thống" },
                        new ModuleModel { Id = "permission", ModuleName = "Phân quyền hệ thống" },
                        new ModuleModel { Id = "allModule", ModuleName = "Tất cả module" }
                    );
                    await context.SaveChangesAsync();
                }

                if (!context.Actions.Any())
                {
                    context.Actions.AddRange(
                        new ActionModel { Id = "create", ActionName = "Tạo mới" },
                        new ActionModel { Id = "update", ActionName = "Chỉnh sửa" },
                        new ActionModel { Id = "delete", ActionName = "Xóa" },
                        new ActionModel { Id = "view", ActionName = "Xem" },
                        new ActionModel { Id = "fullAuthority", ActionName = "Toàn quyền" }
                    );
                    await context.SaveChangesAsync();
                }

                if (!context.Roles.Any())
                {
                    var roleAdmin = new ApplicationRole();
                    roleAdmin.Id = AppRole.Admin;
                    roleAdmin.Name = AppRole.Admin;
                    roleAdmin.NormalizedName = AppRole.Admin.ToUpper();
                    roleAdmin.Description = "Quản lý toàn bộ hệ thống";
                    var result = await roleManager.CreateAsync(roleAdmin);
                    if (!result.Succeeded)
                    {
                        throw new Exception("Failed to create Admin role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    }

                    var modules = context.Modules.ToList();
                    var actions = context.Actions.ToList();
                    var roleModuleAction = new RoleModuleActionModel();


                    roleModuleAction.RoleId = AppRole.Admin;
                    roleModuleAction.ModuleId = "allModule";
                    roleModuleAction.ActionId = "fullAuthority";
                    context.RoleModuleActions.Add(roleModuleAction);
                    await context.SaveChangesAsync();
                }
                if(!context.Users.Any())
                {
                    var user = new ApplicationUser
                    {
                        FirstName = "Admin",
                        LastName = "Default",
                        Email = "Admin@gmail.com",
                        UserName = "Admin",
                    };
                    var password = "Abc@123";

                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        var role = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == AppRole.Admin);
                        if (role != null)
                        {
                            await userManager.AddToRoleAsync(user, role.Name);
                        }                       
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during seeding: {ex.Message}");
                throw;
            }
        }
    }
}