using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public interface IRateInsuranceRepository
    {
        Task<RateInsuranceModel> GetRateInsuranceAsync();
        Task<IdentityResult> UpdateBHYTRateAsync(RateInsuranceModel model);
        Task<IdentityResult> UpdateBHXHRateAsync(RateInsuranceModel model);
        Task<IdentityResult> UpdateBHTNRateAsync(RateInsuranceModel model);
    }
}
