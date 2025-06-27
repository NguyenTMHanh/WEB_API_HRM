using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public interface IDeductionLevelRepository
    {
        Task<DeductionLevelModel> GetDeductionLevelAsync();
        Task<IdentityResult> UpdateDeductionLevelAsync(DeductionLevelModel model);
    }
}
