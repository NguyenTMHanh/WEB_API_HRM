using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IdentityResult> CreatePersonalEmployeeAsync(CreatePersonalEmployeeDto model);
    }
}
