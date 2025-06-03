using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Models;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IdentityResult> CreatePersonalEmployeeAsync(CreatePersonalEmployeeDto model);
        Task<IdentityResult> CreatePersonelEmployeeAsync(CreatePersonelEmployeeDto model);
        Task<List<CodeNameEmployeeRes>> GetCodeNameEmployeeAsync();
        Task<List<CodeNameEmployeeRes>> GetCodeNameManagerAsync(string employeeCode, string rankName);
        Task<GenderDayOfBirthRes> GetGenderDayOfBirthAsync(string employeeCode);
        Task<AccountDefaultRes> GetAccountDefaultAsync(string employeeCode);
        Task<string> GetRoleEmployee(string jobtitleName);
    }
}
