using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public interface IBasicSettingSalaryRepository
    {
        Task<BasicSettingSalaryModel> GetBasicSettingSalaryAsync();
        Task<IdentityResult> UpdateBasicSettingSalary(BasicSettingSalaryModel model);
    }
}
