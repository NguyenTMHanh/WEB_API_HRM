using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public interface IMinimumWageAreaRepository
    {
        Task<IEnumerable<MinimumWageAreaModel>> GetAllMinimumWageAreaAsync();
        Task<IdentityResult> UpdateMinimumWageArea(List<MinimumWageAreaModel> models);
    }
}
