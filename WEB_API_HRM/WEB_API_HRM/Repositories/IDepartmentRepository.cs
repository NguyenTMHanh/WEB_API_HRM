using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IdentityResult> CreateDepartmentAsync(DepartmentModel model);
        Task<IdentityResult> DeleteDepartmentAsync(string departmentId);
        Task<IdentityResult> UpdateDepartmentAsync(string departmentId, DepartmentModel model);
        Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync();
        Task<DepartmentModel> GetDepartmentAsync(string departmentId);
        Task<string> GetCodeDepartment();
    }
}
