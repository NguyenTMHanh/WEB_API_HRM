using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEB_API_HRM.Helpers;

namespace WEB_API_HRM.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HRMContext _context;

        public RoleRepository(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, HRMContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> CreateRoleAsync(ApplicationRole model)
        {
            var existingRole = await _roleManager.FindByNameAsync(model.Name);
            if (existingRole != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role already exists in the system." });
            }

            var role = new ApplicationRole
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                RoleModuleActions = new List<RoleModuleActionModel>()
            };

            if (model.RoleModuleActions != null && model.RoleModuleActions.Any())
            {
                var roleModuleActions = new List<RoleModuleActionModel>();

                foreach (var rma in model.RoleModuleActions)
                {
                    var module = await _context.Modules.FindAsync(rma.ModuleId);
                    if (module == null)
                    {
                        return IdentityResult.Failed(new IdentityError
                        {
                            Description = $"ModuleId '{rma.ModuleId}' does not exist.",
                            Code = CustomCodes.ModuleNotFound.ToString()
                        });
                    }

                    var action = await _context.Actions.FindAsync(rma.ActionId);
                    if (action == null)
                    {
                        return IdentityResult.Failed(new IdentityError
                        {
                            Description = $"ActionId '{rma.ActionId}' does not exist.",
                            Code = CustomCodes.ActionNotFound.ToString()
                        });
                    }

                    roleModuleActions.Add(new RoleModuleActionModel
                    {
                        RoleId = role.Id,
                        ModuleId = rma.ModuleId,
                        ActionId = rma.ActionId
                    });
                }

                _context.RoleModuleActions.AddRange(roleModuleActions);
            }

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return result;
            }

            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<(IdentityResult Result, string UserId)> UpdateRoleAsync(ApplicationRole model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                return (IdentityResult.Failed(new IdentityError { Description = "Role not found" }), null);
            }

            role.Name = model.Name;
            role.Description = model.Description;

            var existingRMAs = _context.RoleModuleActions.Where(r => r.RoleId == role.Id).ToList();
            _context.RoleModuleActions.RemoveRange(existingRMAs);

            if (model.RoleModuleActions != null)
            {
                foreach (var rma in model.RoleModuleActions)
                {
                    var module = await _context.Modules.FindAsync(rma.ModuleId);
                    var action = await _context.Actions.FindAsync(rma.ActionId);

                    if (module == null || action == null)
                    {
                        return (IdentityResult.Failed(new IdentityError { Description = $"ModuleId '{rma.ModuleId}' or ActionId '{rma.ActionId}' does not exist." }), null);
                    }

                    var newRma = new RoleModuleActionModel
                    {
                        RoleId = role.Id,
                        ModuleId = rma.ModuleId,
                        ActionId = rma.ActionId
                    };

                    _context.RoleModuleActions.Add(newRma);
                }
                await _context.SaveChangesAsync();
            }

            var result = await _roleManager.UpdateAsync(role);

            // Tìm tất cả user thuộc vai trò này
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            var userIds = usersInRole.Select(u => u.Id).ToList();

            // Trả về kết quả và danh sách userId
            return (result, userIds.FirstOrDefault());
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role not found" });
            }

            var existingRMAs = _context.RoleModuleActions.Where(r => r.RoleId == roleId).ToList();
            _context.RoleModuleActions.RemoveRange(existingRMAs);
            await _context.SaveChangesAsync();

            return await _roleManager.DeleteAsync(role);
        }
        public async Task<ApplicationRole> GetRoleAsync(string roleId)
        {
            var role = await _roleManager.Roles
                .Include(r => r.RoleModuleActions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role != null)
            {
                return new ApplicationRole
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    RoleModuleActions = role.RoleModuleActions
                        .Select(rma => new RoleModuleActionModel
                        {
                            RoleId = rma.RoleId,
                            ModuleId = rma.ModuleId,
                            ActionId = rma.ActionId
                        }).ToList()
                };
            }
            return null;
        }
        public async Task<List<object>> GetRoleUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null; 
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Any())
            {
                return null; 
            }

            var roleName = userRoles.First();
            var role = await _roleManager.Roles
                .Include(r => r.RoleModuleActions)
                .FirstOrDefaultAsync(r => r.Name == roleName);

            if (role != null)
            {
                var moduleActionList = role.RoleModuleActions
                    .Select(rma => new
                    {
                        ModuleId = rma.ModuleId,
                        ActionId = rma.ActionId
                    })
                    .ToList<object>();

                return moduleActionList;
            }

            return null;
        }
        public async Task<IEnumerable<ApplicationRole>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles
                .Include(r => r.RoleModuleActions)
                .ToListAsync();

            return roles.Select(r => new ApplicationRole
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                RoleModuleActions = r.RoleModuleActions
                    .Select(rma => new RoleModuleActionModel
                    {
                        RoleId = rma.RoleId,
                        ModuleId = rma.ModuleId,
                        ActionId = rma.ActionId
                    }).ToList()
            });
        }
        public async Task<string> GetCodeRole()
        {
            try
            {
                var roleCount = await _roleManager.Roles.CountAsync();
                var nextRoleNumber = roleCount + 1;
                var roleCode = $"ROLE{nextRoleNumber:D3}";
                return roleCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating role code: {ex.Message}");
            }
        }

    }
}