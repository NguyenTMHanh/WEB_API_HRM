using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public interface IAllowanceRepository
    {
        Task<IEnumerable<AllowanceModel>> GetAllAllowanceAsync();
        Task<IdentityResult> UpdateAllowance(List<AllowanceModel> models);
    }
}
