using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Data;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public interface IRoleRepository
    {
        Task<IdentityResult> CreateRoleAsync(ApplicationRole model);
        Task<(IdentityResult Result, string UserId)> UpdateRoleAsync(ApplicationRole model);
        Task<IdentityResult> DeleteRoleAsync(string roleId);
        Task<ApplicationRole> GetRoleAsync(string roleId);
        Task<List<object>> GetRoleUserAsync(string userId);
        Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();
        Task<string> GetCodeRole();
    }
}