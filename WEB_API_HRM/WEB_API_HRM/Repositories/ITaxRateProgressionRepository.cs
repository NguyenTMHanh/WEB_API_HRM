using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public interface ITaxRateProgressionRepository
    {
        Task<IEnumerable<TaxRateProgressionModel>> GetAllTaxRateProgression();
        Task<IdentityResult> UpdateTaxRateProgression(List<TaxRateProgressionModel> models);
    }
}
